using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ImperaPlus.Application.Visibility;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.DTO.Games.Play;
using Game = ImperaPlus.Domain.Games.Game;

namespace ImperaPlus.Application.Play
{
    public interface IPlayService
    {
        DTO.Games.GameActionResult Place(long gameId, IEnumerable<PlaceUnitsOptions> places);

        DTO.Games.GameActionResult Attack(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits);

        DTO.Games.GameActionResult Move(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits);

        DTO.Games.GameActionResult EndAttack(long gameId);

        DTO.Games.Game EndTurn(long gameId);

        DTO.Games.Game Exchange(long gameId);
    }

    public class PlayService : BaseGameService, IPlayService
    {
        public PlayService(IUnitOfWork unitOfWork, IUserProvider userProvider, IVisibilityModifierFactory visibilityModifierFactory, IMapTemplateProvider mapTemplateProvider, IEventAggregator eventAggregator)
            : base(unitOfWork, userProvider, visibilityModifierFactory, mapTemplateProvider, eventAggregator)
        {
        }

        public DTO.Games.GameActionResult Place(long gameId, IEnumerable<PlaceUnitsOptions> places)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.PlaceUnits(places.Select(x => Tuple.Create(x.CountryIdentifier, x.NumberOfUnits)).ToList());

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.GameActionResult Attack(long gameId, string originCountryIdentifier, string destinationCountryIdentifier, int numberOfUnits)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.Attack(originCountryIdentifier, destinationCountryIdentifier, numberOfUnits);

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

            game.Move(originCountryIdentifier, destinationCountryIdentifier, numberOfUnits);

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.GameActionResult EndAttack(long gameId)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.EndAttack();

            return this.CommitAndGetGameActionResult(game);
        }

        public DTO.Games.Game Exchange(long gameId)
        {
            var game = this.GetGame(gameId);
            this.CheckPermission(game);

            game.ExchangeCards();

            this.UnitOfWork.Commit();

            return this.MapAndApplyModifiers(game);
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
            var gameActionResult = Mapper.Map<Domain.Games.Game, DTO.Games.GameActionResult>(game);

            var changedCountries = game.Map.ChangedCountries.ToList();
            foreach(var visibilityModifier in game.Options.VisibilityModifier)
            {
                var visibilityModifierInstance = this.visibilityModifierFactory.Construct(visibilityModifier);

                visibilityModifierInstance.Expand(this.CurrentUser, game, changedCountries);
            }

            gameActionResult.CountryUpdates = Mapper.Map<IEnumerable<DTO.Games.Map.Country>>(changedCountries.ToArray()).ToArray();
            this.UnitOfWork.Commit();

            return gameActionResult;
        }        
    }
}
