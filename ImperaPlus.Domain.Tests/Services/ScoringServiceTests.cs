using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ImperaPlus.Domain.Tests.Services
{
    [TestClass]
    public class ScoringServiceTests
    {
        [TestMethod]
        public void WillUpdateScoreForOneOnOne()
        {
            // Arrange
            var ladder = new Ladder("Default", 2, 1);
            var scoringService = new ScoringService(TestUtils.GetUnitOfWork());

            var game = TestUtils.CreateStartedGameWithMapAndPlayersUnitsPlaced();
            game.Teams.First().Players.First().Surrender();
            
            game.LadderId = ladder.Id;
            game.Ladder = ladder;

            // Act
            scoringService.Score(ladder, game);

            // Assert
            //Assert.AreEqual(2, ladder.Standings.Count, "Player standings have not been created");

            //var player1Standing = ladder.Standings.First(x => x.UserId == game.Teams.First().Players.First().User.Id);
            //var player2Standing = ladder.Standings.First(x => x.UserId == game.Teams.Last().Players.First().User.Id);

            //Assert.IsTrue(player1Standing.Rating < player2Standing.Rating);
            //Assert.AreEqual(1, player1Standing.GamesLost);
            //Assert.AreEqual(1, player1Standing.GamesPlayed);
        }
    }
}
