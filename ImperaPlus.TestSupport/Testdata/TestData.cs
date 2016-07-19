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
            this.GameService = gameService;
        }

        public User CreateUser(string name)
        {
            var user = new User()
            {
                UserName = name
            };

            this.context.Users.Add(user);

            this.SaveChanges();

            return user;
        }

        private void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public Game CreateGame(int teams = 2, int playerPerTeam = 1)
        {
            var mapTemplate = this.CreateAndSaveMapTemplate();

            var game = this.GameService.Create(
                GameType.Fun,
                this.CreateUser("Test"),                
                "NewGame", 60 * 10, mapTemplate.Name, playerPerTeam, teams, new[] { VictoryConditionType.Survival },
                new[] { VisibilityModifierType.None });

            this.context.Games.Add(game);

            this.SaveChanges();

            return game;
        }

        public MapTemplate CreateAndSaveMapTemplate()
        {
            var mapTemplate = Maps.WorldDeluxe();

            if (!this.context.MapTemplates.Any(x => x.Name == "WorldDeluxe"))
            {
                this.context.MapTemplates.Add(new MapTemplateDescriptor
                {
                    Name = "WorldDeluxe"
                });
                this.SaveChanges();
            }

            return mapTemplate;
        }

        public Game CreateGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var users = Enumerable.Range(0, teams * playerPerTeam).Select(x => this.CreateUser("User" + x)).ToArray();

            var game = this.CreateGame(teams, playerPerTeam);

            for (int t = 0; t < teams; ++t)
            {
                var team = game.AddTeam();

                for (int player = 0; player < playerPerTeam; ++player)
                {
                    team.AddPlayer(users[t * playerPerTeam + player]);
                }
            }

            this.SaveChanges();

            return game;
        }

        public Game CreateStartedGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var game = this.CreateGameWithMapAndPlayers(teams, playerPerTeam);

            game.Start();

            this.SaveChanges();

            return game;
        }

        public Game CreateStartedGameWithMapAndPlayersUnitsPlaced(int teams = 2, int playerPerTeam = 1)
        {
            var game = this.CreateStartedGameWithMapAndPlayers(teams, playerPerTeam);

            // Place units
            for (int i = 0; i < teams * playerPerTeam; ++i)
            {
                var currentPlayer = game.CurrentPlayer;

                var countries = new List<Tuple<string, int>>
                {
                    Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, game.GetUnitsToPlace(currentPlayer))
                };

                game.PlaceUnits(countries);
            }

            this.SaveChanges();

            return game;
        }
    }
}
