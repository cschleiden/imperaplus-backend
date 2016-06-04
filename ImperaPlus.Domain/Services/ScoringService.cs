using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Services.Scoring;

namespace ImperaPlus.Domain.Services
{
    public interface IScoringService
    {
        void Score(Ladder ladder, Games.Game game);
    }

    public class ScoringService : IScoringService
    {
        private const int MAX_RANK = 100;

        public void Score(Ladder ladder, Games.Game game)
        {
            var playerStandings = new Dictionary<string, LadderStanding>();

            var winningTeam = game.Teams.First(x => x.Players.First().Outcome == Enums.PlayerOutcome.Won);
            var otherTeams = game.Teams.Where(t => t != winningTeam);

            var winningScoreTeam = new ScoreTeam();
            TransformPlayers(ladder, playerStandings, winningTeam, winningScoreTeam);

            var otherScoreTeams = new List<ScoreTeam>();
            foreach(var otherTeam in otherTeams)
            {
                var otherScoreTeam = new ScoreTeam();
                TransformPlayers(ladder, playerStandings, otherTeam, otherScoreTeam);
                otherScoreTeams.Add(otherScoreTeam);
            }

            var result = new Glicko2().Calculate(winningScoreTeam, otherScoreTeams.ToArray()).SelectMany(x => x.Players).ToDictionary(x => x.Id);
            foreach (var user in game.Teams.SelectMany(t => t.Players.Select(x => x.User)))
            {
                var data = result[user.Id];

                bool hasWon = winningTeam.Players.Any(x => x.UserId == user.Id);
              
                this.UpdatePlayerRating(ladder, playerStandings[user.Id], user, data.Rating, data.Vol, data.Rd, hasWon);
            }
        }

        private static void TransformPlayers(Ladder ladder, Dictionary<string, LadderStanding> playerStandings, Games.Team team, ScoreTeam scoreTeam)
        {
            foreach (var winningPlayer in team.Players)
            {
                var scorePlayer = new ScorePlayer(winningPlayer.UserId);

                // TODO: This makes individual queries, optimize
                var standing = winningPlayer.User.Standings.FirstOrDefault(x => x.LadderId == ladder.Id);
                if (standing != null)
                {
                    scorePlayer.Rating = standing.Rating;
                    scorePlayer.Vol = standing.Vol;
                    scorePlayer.Rd = standing.Rd;
                }

                playerStandings.Add(winningPlayer.UserId, standing);

                scoreTeam.Players.Add(scorePlayer);
            }
        }

        public virtual void UpdatePlayerRating(Ladder ladder, LadderStanding standing, User user, double rating, double vol, double rd, bool hasWon)
        {
            if (standing == null)
            {
                standing = new LadderStanding(ladder, user);
                ladder.Standings.Add(standing);
            }

            standing.Rating = rating;
            standing.Vol = vol;
            standing.Rd = rd;

            standing.GamesPlayed++;
            if (hasWon)
            {
                standing.GamesWon++;
            }
            else
            {
                standing.GamesLost++;
            }

            standing.LastGame = DateTime.UtcNow;
        }
    }
}
