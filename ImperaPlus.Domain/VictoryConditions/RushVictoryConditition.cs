using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;
using System.Linq;

namespace ImperaPlus.Domain.VictoryConditions
{
    public class RushVictoryCondition : SurvivalVictoryCondition
    {
        public override VictoryConditionResult Evaluate(Player player, Games.Map map)
        {
            var result = base.Evaluate(player, map);
            if (result != VictoryConditionResult.Inconclusive)
            {
                return result;
            }

            if (player.Game.TurnCounter < 50)
            {
                return VictoryConditionResult.Inconclusive;
            }

            var ranking = player.Game.Teams.Select(t => new
                {
                    t.Id,
                    Countries = map.GetCountriesForTeam(t.Id).Count(),
                    Units = map.GetCountriesForTeam(t.Id).Sum(x => x.Units)
                })
                .OrderByDescending(x => x.Countries)
                .ThenByDescending(x => x.Units)
                .ThenByDescending(x => x.Id);

            if (player.TeamId == ranking.First().Id)
            {
                // Player's team wins
                return VictoryConditionResult.TeamVictory;
            }

            // Team is not first -> they have lost
            return VictoryConditionResult.TeamDefeat;
        }
    }
}
