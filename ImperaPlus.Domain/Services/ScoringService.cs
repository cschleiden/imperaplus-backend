﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Ladders;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services.Scoring;
using NLog.Fluent;

namespace ImperaPlus.Domain.Services
{
    public interface IScoringService
    {
        void Score(Ladder ladder, Games.Game game);
    }

    public class ScoringService : IScoringService
    {
        private const int MAX_RANK = 100;

        private IUnitOfWork unitOfWork;

        public ScoringService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Score(Ladder ladder, Games.Game game)
        {
            if (game.LadderScored.HasValue && game.LadderScored.Value)
            {
                throw new DomainException(ErrorCode.GameAlreadyScored, $"Game {game.Id} already scored");
            }

            var playerStandings = new Dictionary<string, LadderStanding>();

            var winningTeam = game.Teams.First(x => x.Players.First().Outcome == Enums.PlayerOutcome.Won);
            var otherTeams = game.Teams.Where(t => t != winningTeam);

            // Score winners
            var winningScoreTeam = new ScoreTeam();
            TransformPlayers(ladder, playerStandings, winningTeam, winningScoreTeam);

            // Score losers
            var otherScoreTeams = new List<ScoreTeam>();
            foreach (var otherTeam in otherTeams)
            {
                var otherScoreTeam = new ScoreTeam();
                TransformPlayers(ladder, playerStandings, otherTeam, otherScoreTeam);
                otherScoreTeams.Add(otherScoreTeam);
            }

            // Calculate new ratings for each player
            var result = new Glicko2()
                .Calculate(winningScoreTeam, otherScoreTeams.ToArray())
                .SelectMany(x => x.Players)
                .ToDictionary(x => x.Id);
            foreach (var user in game.Teams.SelectMany(t => t.Players.Select(x => x.User)))
            {
                var data = result[user.Id];

                var hasWon = winningTeam.Players.Any(x => x.UserId == user.Id);

                UpdatePlayerRating(ladder, playerStandings[user.Id], user, data.Rating, data.Vol, data.Rd, hasWon);
            }

            game.LadderScored = true;
        }

        private void TransformPlayers(Ladder ladder, Dictionary<string, LadderStanding> playerStandings,
            Games.Team team, ScoreTeam scoreTeam)
        {
            foreach (var player in team.Players)
            {
                var scorePlayer = new ScorePlayer(player.UserId);

                // TODO: This makes individual queries, optimize                
                //var standing = this.unitOfWork.Ladders.GetUserStanding(ladder.Id, player.UserId);
                var standing = unitOfWork.GetGenericRepository<LadderStanding>()
                    .Query()
                    .FirstOrDefault(x => x.LadderId == ladder.Id && x.UserId == player.UserId);
                if (standing != null)
                {
                    Log.Info().Message("Found ladder standing for {0} {1}", ladder.Id, player.UserId).Write();

                    // Player has already competed in this ladder
                    scorePlayer.Rating = standing.Rating;
                    scorePlayer.Vol = standing.Vol;
                    scorePlayer.Rd = standing.Rd;
                }

                playerStandings.Add(player.UserId, standing);

                scoreTeam.Players.Add(scorePlayer);
            }
        }

        public virtual void UpdatePlayerRating(Ladder ladder, LadderStanding standing, User user, double rating,
            double vol, double rd, bool hasWon)
        {
            if (standing == null)
            {
                // Player has competed in this league for the first time
                Log.Info().Message("Adding ladder standing for {0} {1}", ladder.Id, user.Id).Write();
                standing = new LadderStanding(ladder, user);
                unitOfWork.GetGenericRepository<LadderStanding>().Add(standing);
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
