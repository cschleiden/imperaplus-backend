using ImperaPlus.Domain.Services.Scoring;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Tests.Services.Scoring
{
    [TestClass]
    public class Glicko2Tests
    {
        [TestMethod]
        public void Calculate1vs1()
        {
            // Arrange           
            var winningTeam = new ScoreTeam()
            {
                Players =
                {
                    new ScorePlayer("42", 1500, 200.0, 0.06)
                }
            };

            var losingTeam = new ScoreTeam()
            {
                Players =
                {
                    new ScorePlayer("23", 1400, 81.0, 0.06)
                }
            };

            // Act 1
            var updatedTeams = new Glicko2().Calculate(winningTeam, losingTeam);
            this.AssertEqualEpsilon(1562.1819, updatedTeams.First().Players.First().Rating);
            this.AssertEqualEpsilon(176.4211, updatedTeams.First().Players.First().Rd);

            this.AssertEqualEpsilon(1385.90212, updatedTeams.Last().Players.Last().Rating);
            this.AssertEqualEpsilon(80.1919, updatedTeams.Last().Players.Last().Rd);
        }

        [TestMethod]
        public void Calculate4PlayerFFA()
        {
            var winningTeam = new ScoreTeam()
            {
                Players =
                {
                    new ScorePlayer("A", 1500, 200.0, 0.06),
                    new ScorePlayer("B", 1500, 200.0, 0.06)
                }
            };

            var losingTeam = new ScoreTeam()
            {
                Players =
                {
                    new ScorePlayer("A", 1500, 200.0, 0.06),
                    new ScorePlayer("B", 1500, 200.0, 0.06)
                }
            };

            var result = new Glicko2().Calculate(winningTeam, losingTeam);
        }

        [TestMethod]
        public void Calculate2vs2()
        {

        }

        private void AssertEqualEpsilon(double expected, double actual)
        {
            Assert.IsTrue(expected > actual - 0.001 && expected < actual + 0.001, string.Format("Expected: {0}, Actual: {1}", expected, actual));
        }
    }
}
