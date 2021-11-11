using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ImperaPlus.DataAccess;
using ImperaPlus.Domain;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Enums;
using ImperaPlus.DataAccess.ConvertedMaps;

namespace ImperaPlus.TestSupport.Testdata
{
    public class TestData
    {
        private readonly IImperaContext context;
        private readonly IComponentContext componentContext;
        public IGameService GameService { get; private set; }

        public TestData(IImperaContext context, IComponentContext componentContext, IGameService gameService)
        {
            this.context = context;
            this.componentContext = componentContext;
            GameService = gameService;
        }

        public User CreateUser(string name)
        {
            var user = new User() { UserName = name };

            context.Users.Add(user);

            SaveChanges();

            return user;
        }

        private void SaveChanges()
        {
            context.SaveChanges();
        }

        public Game CreateGame(int teams = 2, int playerPerTeam = 1)
        {
            var mapTemplate = CreateAndSaveMapTemplate();

            var game = GameService.Create(
                GameType.Fun,
                CreateUser("Test"),
                "NewGame",
                null,
                60 * 10, mapTemplate.Name, playerPerTeam, teams, new[] { VictoryConditionType.Survival },
                new[] { VisibilityModifierType.None });

            context.Games.Add(game);

            SaveChanges();

            return game;
        }

        public MapTemplate CreateAndSaveMapTemplate()
        {
            var mapTemplate = Maps.WorldDeluxe();

            if (!context.MapTemplates.Any(x => x.Name == "WorldDeluxe"))
            {
                context.MapTemplates.Add(new MapTemplateDescriptor { Name = "WorldDeluxe" });
                SaveChanges();
            }

            return mapTemplate;
        }

        public Game CreateGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var users = Enumerable.Range(0, teams * playerPerTeam).Select(x => CreateUser("User" + x)).ToArray();

            var game = CreateGame(teams, playerPerTeam);

            for (var t = 0; t < teams; ++t)
            {
                var team = game.AddTeam();

                for (var player = 0; player < playerPerTeam; ++player)
                {
                    team.AddPlayer(users[t * playerPerTeam + player]);
                }
            }

            SaveChanges();

            return game;
        }

        public Game CreateStartedGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var game = CreateGameWithMapAndPlayers(teams, playerPerTeam);

            game.Start(CreateAndSaveMapTemplate(), new TestRandomGen());

            SaveChanges();

            return game;
        }

        public Game CreateStartedGameWithMapAndPlayersUnitsPlaced(int teams = 2, int playerPerTeam = 1)
        {
            var game = CreateStartedGameWithMapAndPlayers(teams, playerPerTeam);

            // Place units
            for (var i = 0; i < teams * playerPerTeam; ++i)
            {
                var currentPlayer = game.CurrentPlayer;

                var countries = new List<Tuple<string, int>>
                {
                    Tuple.Create(currentPlayer.Countries.First().CountryIdentifier,
                        game.GetUnitsToPlace(CreateAndSaveMapTemplate(), currentPlayer))
                };

                game.PlaceUnits(CreateAndSaveMapTemplate(), countries);
            }

            SaveChanges();

            return game;
        }
    }
}
