using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Map;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using Moq;

namespace ImperaPlus.Domain.Tests
{
    public static class TestUtils
    {
        public static User CreateUser(string name)
        {
            return new User()
            {
                UserName = name
            };
        }

        public static IMapTemplateProvider MockMapTemplateProvider()
        {
            var mockMapTemplateProvider = new Mock<IMapTemplateProvider>();

            mockMapTemplateProvider
                .Setup(x => x.GetTemplate(It.IsAny<string>()))
                .Returns(GetMapTemplate());

            return mockMapTemplateProvider.Object;
        }

        public static MapTemplate GetMapTemplate()
        {
            var mapTemplate = ImperaPlus.DataAccess.ConvertedMaps.Maps.WorldDeluxe();

            return mapTemplate;
        }

        public static IEventAggregator GetEventAggregator()
        {
            var mockEventAggregator = new Mock<IEventAggregator>();

            return mockEventAggregator.Object;
        }

        public static Mock<IUnitOfWork> GetUnitOfWorkMock()
        {
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            var mockUserRepository = new Mock<IUserRepository>();
            var systemUser = CreateUser("System");
            mockUserRepository.Setup(x => x.FindByName("System")).Returns(systemUser);
            mockUnitOfWork.SetupGet(x => x.Users).Returns(mockUserRepository.Object);

            var mockPlayerRepository = new Mock<IPlayerRepository>();
            mockUnitOfWork.SetupGet(x => x.Players).Returns(mockPlayerRepository.Object);

            var mockTeamRepository = new Mock<ITeamRepository>();
            mockUnitOfWork.SetupGet(x => x.Teams).Returns(mockTeamRepository.Object);

            var mockLadderQueueEntry = new Mock<IGenericRepository<Domain.Ladders.LadderQueueEntry>>();

            mockUnitOfWork.Setup(x => x.GetGenericRepository<Domain.Ladders.LadderQueueEntry>()).Returns(mockLadderQueueEntry.Object);

            return mockUnitOfWork;
        }

        public static IUnitOfWork GetUnitOfWork()
        {
            return GetUnitOfWorkMock().Object;
        }

        public static IContainer Container;

        public static Game CreateGame(int teams = 2, int playerPerTeam = 1, Enums.GameType type = GameType.Fun)
        {
            var mapTemplate = new MapTemplate("blah");

            var game = new Game(
                CreateUser("Test"),
                type,
                "NewGame", 
                mapTemplate.Name, 
                60 * 10, 
                teams,
                playerPerTeam, 
                new[] { VictoryConditionType.Survival },
                new[] { VisibilityModifierType.None });

            return game;
        }

        public static Game CreateGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var users = Enumerable.Range(0, teams * playerPerTeam).Select(x => CreateUser("User" + x)).ToArray();

            var game = CreateGame(teams, playerPerTeam);

            for (int t = 0; t < teams; ++t)
            {
                var team = game.AddTeam();

                for (int player = 0; player < playerPerTeam; ++player)
                {
                    team.AddPlayer(users[t * playerPerTeam + player]);
                }
            }          

            return game;
        }

        public static Game CreateStartedGameWithMapAndPlayers(int teams = 2, int playerPerTeam = 1)
        {
            var game = CreateGameWithMapAndPlayers(teams, playerPerTeam);

            game.Start(TestUtils.GetMapTemplate());

            return game;
        }

        public static Game CreateStartedGameWithMapAndPlayersUnitsPlaced(int teams = 2, int playerPerTeam = 1)
        {
            var game = CreateStartedGameWithMapAndPlayers(teams, playerPerTeam);

            // Place units
            for (int i = 0; i < teams * playerPerTeam; ++i)
            {
                var currentPlayer = game.CurrentPlayer;

                var countries = new List<Tuple<string, int>>
                {
                    Tuple.Create(currentPlayer.Countries.First().CountryIdentifier, game.GetUnitsToPlace(TestUtils.GetMapTemplate(), currentPlayer))
                };

                game.PlaceUnits(TestUtils.GetMapTemplate(), countries);
            }

            return game;
        }
    }
}
