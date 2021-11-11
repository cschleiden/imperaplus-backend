﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tournaments;
using System;
using ImperaPlus.Domain.Exceptions;

namespace ImperaPlus.Domain.Services
{
    public interface ITournamentService
    {
        /// <summary>
        /// Check whether tournaments can be started and start
        /// </summary>
        bool CheckOpenTournaments(ILogger log, IRandomGen random);

        /// <summary>
        /// Synchronize tournament games and end tournaments
        /// </summary>
        void CheckTournaments(ILogger log, IRandomGen random);

        /// <summary>
        /// Get games for the given pairing
        /// </summary>
        IEnumerable<Game> GetGamesForPairing(Guid pairingId);
    }

    public class TournamentService : ITournamentService
    {
        private IUserProvider userProvider;
        private IUnitOfWork unitOfWork;
        private IGameService gameService;
        private IMapTemplateProvider mapTemplateProvider;
        private User ghostUser;

        public TournamentService(
            IUserProvider userProvider,
            IUnitOfWork unitOfWork,
            IGameService gameService,
            IMapTemplateProvider mapTemplateProvider)
        {
            this.userProvider = userProvider;
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
            this.mapTemplateProvider = mapTemplateProvider;

            ghostUser = unitOfWork.Users.FindByName("Ghost");
        }

        /// <summary>
        /// Check all open tournaments whether they can be started
        /// </summary>
        /// <returns>True if a tournament was started</returns>
        public bool CheckOpenTournaments(ILogger log, IRandomGen random)
        {
            var tournamentStarted = false;

            var tournaments = unitOfWork.Tournaments.Get(TournamentState.Open);

            foreach (var tournament in tournaments)
            {
                if (tournament.CanStart)
                {
                    log.Log(LogLevel.Info, "Starting tournament {0}", tournament.Id);

                    tournament.Start(random);
                    tournamentStarted = true;

                    log.Log(LogLevel.Info, "Started.");
                }
            }

            return tournamentStarted;
        }

        /// <summary>
        /// Check all tournaments to see whether rounds need to advanced, or the tournament ended
        /// </summary>
        public void CheckTournaments(ILogger log, IRandomGen random)
        {
            var tournamentIds = unitOfWork.Tournaments.Get(Tournament.ActiveStates).Select(t => t.Id);

            foreach (var tournamentId in tournamentIds)
            {
                try
                {
                    var tournament = unitOfWork.Tournaments.GetById(tournamentId);

                    log.Log(LogLevel.Info, "Synchronizing games for tournament {0}", tournament.Id);

                    SynchronizeGamesToPairings(log, tournament);

                    if (tournament.HasGroupPhase && tournament.State == TournamentState.Groups)
                    {
                        log.Log(LogLevel.Info, "Update group order for tournament {0}", tournament.Id);

                        OrderGroupTeams(tournament);
                    }

                    // Advance rounds
                    if (tournament.CanStartNextRound)
                    {
                        log.Log(LogLevel.Info, "Starting next round for tournament {0}", tournament.Id);

                        tournament.StartNextRound(random, log);
                    }
                    else if (tournament.CanEnd)
                    {
                        log.Log(LogLevel.Info, "Ending tournament {0}", tournament.Id);

                        tournament.End();
                    }

                    CreateGamesForPairings(log, tournament, random);
                }
                catch (Exception ex)
                {
                    log.Log(LogLevel.Error, "Error while handling tournament {0}: {1}", tournamentId, ex);

                    // Rethrow exception, we have to cancel the job
                    throw;
                }
            }
        }

        public void CreateGamesForPairings(ILogger log, Tournament tournament, IRandomGen random)
        {
            log.Log(LogLevel.Info, "Creating games for {0}", tournament.Id);

            foreach (var pairing in tournament.Pairings.Where(
                         x => x.State == PairingState.None && x.Games.Count() != x.NumberOfGames))
            {
                log.Log(LogLevel.Info, "Creating games for pairing {0}", pairing.Id);

                CreateGamesForPairing(log, pairing, random);
            }
        }

        public void SynchronizeGamesToPairings(ILogger log, Tournament tournament)
        {
            var activePairings = tournament.Pairings.Where(x => x.State == PairingState.Active).ToList();

            log.Log(LogLevel.Info, "Checking {0} active pairings of {1} total", activePairings.Count,
                tournament.Pairings.Count);

            foreach (var pairing in activePairings)
            {
                // Synchronize number of won games
                pairing.TeamAWon = CountWonGamesForTeam(pairing.Games, pairing.TeamA);
                pairing.TeamBWon = CountWonGamesForTeam(pairing.Games, pairing.TeamB);

                log.Log(LogLevel.Info, "Wins: TeamA {0} TeamB {1}", pairing.TeamAWon, pairing.TeamBWon);

                if (pairing.CanWinnerBeDetermined)
                {
                    log.Log(LogLevel.Info, "Found winner for pairing {0}", pairing.Id);

                    pairing.State = PairingState.Done;

                    if (tournament.State == TournamentState.Knockout)
                    {
                        // If the tournament is in knockout mode, losing a pairing means losing the tournament, so update
                        // the loser's team.
                        pairing.Loser.State = TournamentTeamState.InActive;

                        // TODO: Generate domain event for winner
                        // TODO: Generate domain event for loser
                    }
                }
                else
                {
                    log.Log(LogLevel.Info, "Cannot find winner for pairing {0}", pairing.Id);
                }
            }
        }

        public void OrderGroupTeams(Tournament tournament)
        {
            foreach (var group in tournament.Groups)
            {
                // Do initial sorting, count wins
                var wonPairingsByTeam = group.Teams.ToDictionary(x => x.Id, x => 0);
                var wonGamesByTeam = group.Teams.ToDictionary(x => x.Id, x => 0);
                foreach (var pairing in group.Pairings)
                {
                    wonGamesByTeam[pairing.TeamAId] += pairing.TeamAWon;
                    wonGamesByTeam[pairing.TeamBId] += pairing.TeamBWon;

                    if (pairing.CanWinnerBeDetermined)
                    {
                        wonPairingsByTeam[pairing.Winner.Id] += 1;
                    }
                }

                // Sort by:
                // - won pairings
                // - then number of won games
                // - then look at the pairing of the two teams (if it exists)
                // - then fall back to Id (basically random in this context)
                // Note: this is a bit timing dependent, pairings might be won before all games have been played
                // so order could change later after the group phase. This seems acceptable for now.
                var orderedTeams = group.Teams
                    .OrderByDescending(t => wonPairingsByTeam[t.Id])
                    .ThenByDescending(t => wonGamesByTeam[t.Id])
                    .ThenByDescending(t => t, Comparer<TournamentTeam>.Create((teamA, teamB) =>
                    {
                        // Check pairing
                        var factor = 1;
                        var pairing = group.Pairings.FirstOrDefault(x => x.TeamA == teamA && x.TeamB == teamB);
                        if (pairing != null && pairing.CanWinnerBeDetermined)
                        {
                            var t = teamA;
                            teamB = teamA;
                            teamB = t;
                            factor = -1;
                        }

                        pairing = group.Pairings.FirstOrDefault(x => x.TeamA == teamA && x.TeamB == teamB);
                        if (pairing != null && pairing.CanWinnerBeDetermined)
                        {
                            if (pairing.Winner == teamA)
                            {
                                return -1 * factor;
                            }
                            else
                            {
                                return 1 * factor;
                            }
                        }

                        // Fallback to random Id...
                        return teamA.GroupOrder.CompareTo(teamB.GroupOrder);
                    }))
                    .ToList();

                for (var i = 0; i < orderedTeams.Count; ++i)
                {
                    orderedTeams[i].GroupOrder = i + 1;
                }
            }
        }

        private int CountWonGamesForTeam(IEnumerable<Game> games, TournamentTeam team)
        {
            // Return number of games that
            // - have ended
            // - a winning team contains
            //  - the first player of the given team (enough to check)
            return games.Count(x => x.State == Enums.GameState.Ended
                                    && x.Teams.Any(t =>
                                        t.Outcome == Enums.PlayerOutcome.Won
                                        && t.Players.Select(p => p.UserId).Contains(team.Participants.First().UserId)));
        }

        private void CreateGamesForPairing(ILogger log, TournamentPairing pairing, IRandomGen random)
        {
            Debug.Assert(pairing.State == PairingState.None, "Pairing state is not correct");

            var systemUser = unitOfWork.Users.FindByName("System");

            for (var i = 0; i < pairing.NumberOfGames; ++i)
            {
                log.Log(LogLevel.Info, "Creating game {0}", i);

                var gameName = pairing.GenerateGameName(i);
                Game game;

                try
                {
                    game = gameService.Create(
                        Enums.GameType.Tournament,
                        systemUser,
                        gameName,
                        null,
                        pairing.Tournament.GetMapTemplateForGame(random),
                        pairing.Tournament.Options);
                }
                catch (DomainException exception) when (exception.ErrorCode == ErrorCode.NameAlreadyTaken)
                {
                    // Try again
                    game = gameService.Create(
                        Enums.GameType.Tournament,
                        systemUser,
                        gameName + Guid.NewGuid().ToString(),
                        null,
                        pairing.Tournament.GetMapTemplateForGame(random),
                        pairing.Tournament.Options);
                }

                game.TournamentPairingId = pairing.Id;
                pairing.Games.Add(game);

                var teamA = game.AddTeam();
                foreach (var participant in pairing.TeamA.Participants)
                {
                    if (participant.User == null)
                    {
                        teamA.AddPlayer(ghostUser);
                    }
                    else
                    {
                        teamA.AddPlayer(participant.User);
                    }
                }

                var teamB = game.AddTeam();
                foreach (var participant in pairing.TeamB.Participants)
                {
                    if (participant.User == null)
                    {
                        teamB.AddPlayer(ghostUser);
                    }
                    else
                    {
                        teamB.AddPlayer(participant.User);
                    }
                }

                game.Start(mapTemplateProvider.GetTemplate(game.MapTemplateName), random);

                log.Log(LogLevel.Info, "Done.");
            }

            pairing.State = PairingState.Active;
        }

        public IEnumerable<Game> GetGamesForPairing(Guid pairingId)
        {
            return unitOfWork.Tournaments.GetGamesForPairing(pairingId);
        }
    }
}
