using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.DTO.Ladder;

namespace ImperaPlus.Application.Ladder
{
    public interface ILadderService
    {
        IEnumerable<LadderSummary> GetAll();

        IEnumerable<DTO.Ladder.Ladder> GetAllFull();

        DTO.Ladder.Ladder Get(Guid ladderId);

        void Queue(Guid ladderId);

        void LeaveQueue(Guid ladderId);

        LadderSummary Create(DTO.Ladder.Admin.CreationOptions creationOptions);

        void Delete(Guid ladderId);

        void UpdateMapTemplates(Guid id, IEnumerable<string> mapTemplateNames);

        void ToggleActive(Guid ladderId, bool isActive);

        void UpdateGameOptions(Guid ladderId, DTO.Games.GameOptions gameOptions);
    }

    public class LadderService : BaseService, ILadderService
    {
        private Domain.Services.ILadderService ladderService;

        public LadderService(IUnitOfWork unitOfWork, IUserProvider userProvider, Domain.Services.ILadderService ladderService)
            : base(unitOfWork, userProvider)
        {
            this.ladderService = ladderService;
        }

        public LadderSummary Create(DTO.Ladder.Admin.CreationOptions creationOptions)
        {
            var ladder = this.ladderService.Create(creationOptions.Name, creationOptions.NumberOfTeams, creationOptions.NumberOfPlayers);

            this.UnitOfWork.Commit();

            return Mapper.Map<LadderSummary>(ladder);
        }

        public void Delete(Guid ladderId)
        {
            var ladder = this.GetLadder(ladderId);

            this.UnitOfWork.Ladders.Remove(ladder);

            this.UnitOfWork.Commit();
        }

        public void UpdateMapTemplates(Guid id, IEnumerable<string> mapTemplateNames)
        {
            Domain.Ladders.Ladder ladder = GetLadder(id);

            foreach (var mapTemplateName in mapTemplateNames)
            {
                // Ensure MapTemplates exist
                if (this.UnitOfWork.MapTemplateDescriptors.Get(mapTemplateName) == null)
                {
                    throw new Exceptions.ApplicationException("Cannot find map template", ErrorCode.CannotFindMapTemplate);
                }
            }

            ladder.MapTemplates.Clear();
            ladder.MapTemplates.AddRange(mapTemplateNames);

            this.UnitOfWork.Commit();
        }

        public void UpdateGameOptions(Guid id, DTO.Games.GameOptions gameOptions)
        {
            Domain.Ladders.Ladder ladder = GetLadder(id);

            ladder.Options.AttacksPerTurn = gameOptions.AttacksPerTurn;
            ladder.Options.InitialCountryUnits = gameOptions.InitialCountryUnits;
            ladder.Options.MapDistribution = Mapper.Map<Domain.Enums.MapDistribution>(gameOptions.MapDistribution);
            ladder.Options.MaximumNumberOfCards = gameOptions.MaximumNumberOfCards;
            ladder.Options.MinUnitsPerCountry = gameOptions.MinUnitsPerCountry;
            ladder.Options.MovesPerTurn = gameOptions.MovesPerTurn;
            ladder.Options.NewUnitsPerTurn = gameOptions.NewUnitsPerTurn;
            ladder.Options.TimeoutInSeconds = gameOptions.TimeoutInSeconds;

            ladder.Options.VictoryConditions.Clear();
            ladder.Options.VictoryConditions.AddRange(Mapper.Map<IEnumerable<Domain.Enums.VictoryConditionType>>(gameOptions.VictoryConditions));

            ladder.Options.VisibilityModifier.Clear();
            ladder.Options.VisibilityModifier.AddRange(Mapper.Map<IEnumerable<Domain.Enums.VisibilityModifierType>>(gameOptions.VisibilityModifier));

            this.UnitOfWork.Commit();
        }

        public void ToggleActive(Guid ladderId, bool isActive)
        {
            Domain.Ladders.Ladder ladder = GetLadder(ladderId);

            ladder.ToggleActive(isActive);

            this.UnitOfWork.Commit();
        }

        public DTO.Ladder.Ladder Get(Guid ladderId)
        {           
            Domain.Ladders.Ladder ladder = GetLadder(ladderId);
            
            var mappedLadder = Mapper.Map<DTO.Ladder.Ladder>(ladder);

            // Fill in standings here for now
            mappedLadder.Standings = Mapper.Map<DTO.Ladder.LadderStanding[]>(GetStandings(ladderId, 0).ToArray());

            return mappedLadder;
        }

        public IEnumerable<DTO.Ladder.Ladder> GetAllFull()
        {
            return Mapper.Map<IEnumerable<DTO.Ladder.Ladder>>(this.UnitOfWork.Ladders.GetAll());
        }

        public IEnumerable<LadderSummary> GetAll()
        {
            var user = this.CurrentUser;

            var ladderSummaries = new List<LadderSummary>();

            var activeLadders = this.UnitOfWork.Ladders.GetActive().ToList();
            foreach(var activeLadder in activeLadders)
            {
                var ladderSummary = Mapper.Map<LadderSummary>(activeLadder);

                ladderSummary.IsQueued = activeLadder.Queue.Any(x => x.UserId == user.Id);

                var userStanding = this.UnitOfWork.Ladders.GetUserStanding(ladderSummary.Id, user.Id);
                if (userStanding != null)
                {
                    ladderSummary.Standing = new LadderStanding
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Rating = userStanding.Rating,
                        GamesPlayed = userStanding.GamesPlayed,
                        GamesWon = userStanding.GamesWon,
                        GamesLost = userStanding.GamesLost,
                        Position = this.UnitOfWork.Ladders.GetStandingPosition(userStanding),
                    };
                }

                ladderSummaries.Add(ladderSummary);
            }

            return ladderSummaries;
        }

        public void Queue(Guid id)
        {
            var currentUser = this.CurrentUser;

            this.ladderService.Queue(id, currentUser);

            this.UnitOfWork.Commit();
        }

        public void LeaveQueue(Guid ladderId)
        {
            var currentUser = this.CurrentUser;

            this.ladderService.LeaveQueue(ladderId, currentUser);

            this.UnitOfWork.Commit();
        }

        private Domain.Ladders.Ladder GetLadder(Guid id)
        {
            var ladder = this.UnitOfWork.Ladders.GetById(id);
            if (ladder == null)
            {
                throw new Exceptions.ApplicationException("Cannot find ladder", ErrorCode.CannotFindLadder);
            }

            return ladder;
        }

        public IEnumerable<LadderStanding> GetStandings(Guid ladderId, int start, int count = 30)
        {
            // Future: Think about paging
            var standings = this.UnitOfWork.Ladders.GetStandings(ladderId);

            int position = 0;
            return standings.Select(x => new LadderStanding
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                Rating = x.Rating,
                GamesPlayed = x.GamesPlayed,
                GamesWon = x.GamesWon,
                GamesLost = x.GamesLost,
                LastGame = x.LastGame,
                Position = ++position
            });
        }
    }
}
