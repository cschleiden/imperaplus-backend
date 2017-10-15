using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImperaPlus.Domain.Tests.Services
{
    [TestClass]
    public class GameServiceTests
    {
        /*[TestMethod]
        [ExpectedException(typeof(DomainException))]
        public void CreateGameWhenNotEnoughSlotsAreAvailableForCreatingShouldFail()
        {
            // Arrange   
            var user = this.CreateUser("UserWithoutSlots");
            user.GameSlots = 0;

            // Act
            var gameService = new Domain.Services.GameService();
            var game = this.TestData.GameService.Create(GameType.Fun, user, "newgame", 60 * 10, TestData.CreateAndSaveMapTemplate().Name, 1, 2, new[] {
                    VictoryConditionType.Survival
            }, new[] {
                VisibilityModifierType.None
            });

            // Assert            
        }*/
    }
}
