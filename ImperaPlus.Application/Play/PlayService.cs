using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.Application.Visibility;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games.Play;
using Game = ImperaPlus.Domain.Games.Game;

namespace ImperaPlus.Application.Play
{
    public interface IPlayService
    {
        DTO.Games.GameActionResult Exchange(long gameId);

        DTO.Games.GameActionResult Place(long gameId, IEnumerable<PlaceUnitsOptions> places);

        DTO.Games.GameActionResult Attack(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits);

        DTO.Games.GameActionResult Move(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits);

        DTO.Games.GameActionResult EndAttack(long gameId);

        DTO.Games.Game EndTurn(long gameId);
    }

    public class PlayService : BaseGameService, IPlayService
    {
        private IRandomGen randomGen;
        private IAttackService attackService;

        public PlayService(IUnitOfWork unitOfWork, IUserProvider userProvider, IVisibilityModifierFactory visibilityModifierFactory, 
            IAttackService attackService,
            IMapTemplateProvider mapTemplateProvider, 
            IRandomGen randomGen)
            : base(unitOfWork, userProvider, mapTemplateProvider, visibilityModifierFactory)
        {
            this.attackService = attackService;
            this.randomGen = randomGen;
        }

        public DTO.Games.GameActionResult Place(long gameId, IEnumerable<PlaceUnitsOptions> places)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.ResetTracking();

            game.PlaceUnits(
                this.GetMapTemplate(game), places.Select(x => Tuple.Create(x.CountryIdentifier, x.NumberOfUnits)).ToList());

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.GameActionResult Attack(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.ResetTracking();

            game.Attack(this.attackService, this.randomGen, this.GetMapTemplate(game), originCountryIdentifier, destinationCountryIdentifier, numberOfUnits);

            var actionResult = this.CommitAndGetGameActionResult(game);

            var currentPlayer = game.GetPlayerForUser(this.userProvider.GetCurrentUserId());

            var destCountry = actionResult.CountryUpdates.FirstOrDefault(x => x.Identifier == destinationCountryIdentifier);
            actionResult.ActionResult = (destCountry != null && destCountry.PlayerId == currentPlayer.Id)
                ? DTO.Games.ActionResult.Successful : DTO.Games.ActionResult.NotSuccessful;

            return actionResult;
        }

        public DTO.Games.GameActionResult Move(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.ResetTracking();

            game.Move(this.GetMapTemplate(game), originCountryIdentifier, destinationCountryIdentifier, numberOfUnits);

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.GameActionResult EndAttack(long gameId)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.EndAttack();

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.GameActionResult Exchange(long gameId)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.ExchangeCards();
            
            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.Game EndTurn(long gameId)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.EndTurn();

            this.UnitOfWork.Commit();

            return this.MapAndApplyModifiers(game);
        }

        private void CheckPermission(Game game)
        {
            if (game.CurrentPlayer.UserId != this.userProvider.GetCurrentUserId())
            {
                throw new Exceptions.ApplicationException("Only current player can perform actions", ErrorCode.UserIsNotAllowedToPerformAction);
            }
        }

        private DTO.Games.GameActionResult CommitAndGetGameActionResult(Game game)
        {            
            var gameActionResult = Mapper.Map<Domain.Games.Game, DTO.Games.GameActionResult>(game, opts => opts.Items.Add("userId", this.CurrentUserId));

            var changedCountries = game.Map.ChangedCountries.ToList();
            foreach(var visibilityModifier in game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Expand(this.CurrentUser, game, changedCountries);
            }

            gameActionResult.CountryUpdates = Mapper.Map<IEnumerable<DTO.Games.Map.Country>>(changedCountries.ToArray()).ToArray();
            gameActionResult.UnitsToPlace = game.GetUnitsToPlace(this.mapTemplateProvider.GetTemplate(game.MapTemplateName), game.CurrentPlayer);

            this.UnitOfWork.Commit();

            return gameActionResult;
        }   
        
        private MapTemplate GetMapTemplate(Game game)
        {
            return this.mapTemplateProvider.GetTemplate(game.MapTemplateName);
        }
    }
}
