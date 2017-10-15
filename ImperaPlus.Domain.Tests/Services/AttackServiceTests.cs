using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImperaPlus.Domain.Services;
using ImperaPlus.TestSupport;

namespace ImperaPlus.Domain.Tests.Services
{
    [TestClass]
    public class AttackServiceTests
    {
        [TestMethod]
        public void AttackService_AttackerWins()
        {
            // Arrange
            var attackService = new AttackService(new AttackerWinsRandomGen());

            // Act
            int attackerUnits = 3;
            int defenderUnits = 2;
            int attackerUnitsLost;
            int defenderUnitsLost;
            var result = attackService.Attack(
                attackerUnits, defenderUnits, 
                out attackerUnitsLost, out defenderUnitsLost);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(defenderUnits, defenderUnitsLost);
            Assert.AreEqual(0, attackerUnitsLost);
        }

        [TestMethod]
        public void AttackService_DefenderWins()
        {
            // Arrange
            var attackService = new AttackService(new DefenderWinsRandomGen());

            // Act
            int attackerUnits = 2;
            int defenderUnits = 2;
            int attackerUnitsLost;
            int defenderUnitsLost;
            var result = attackService.Attack(
                attackerUnits, defenderUnits,
                out attackerUnitsLost, out defenderUnitsLost);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(attackerUnits, attackerUnitsLost);
            Assert.AreEqual(0, defenderUnitsLost);
        }

        [TestMethod]
        public void AttackService_DefenderWinsForEvenDiceRolls()
        {
            // Arrange
            var randomService = new PredefinedRandomGen(6, 6);
            var attackService = new AttackService(randomService);

            // Act
            int attackerUnits = 2;
            int defenderUnits = 2;
            int attackerUnitsLost;
            int defenderUnitsLost;
            var result = attackService.Attack(
                attackerUnits, defenderUnits,
                out attackerUnitsLost, out defenderUnitsLost);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(attackerUnits, attackerUnitsLost);
            Assert.AreEqual(0, defenderUnitsLost);
        }        
    }
}
