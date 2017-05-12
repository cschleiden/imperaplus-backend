using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tournaments;
using NLog.Fluent;
using System;

namespace ImperaPlus.Domain.Services
{
    public interface ITournamentService
    {
        /// <summary>
        /// Check whether tournaments can be started and start
        /// </summary>
        bool CheckOpenTournaments();

        /// <summary>
        /// Synchronize tournament games and end tournaments
        /// </summary>
        void CheckTournaments();
    }

    public class TournamentService : ITournamentService
    {
        private IGameService gameService;
        private IMapTemplateProvider mapTemplateProvider;
        private IUnitOfWork unitOfWork;
        

        public TournamentService(
            IUnitOfWork unitOfWork, 
            IGameService gameService, 
            IMapTemplateProvider mapTemplateProvider)
        {
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
            this.mapTemplateProvider = mapTemplateProvider;
        }

        public bool CheckOpenTournaments()
        {
            bool tournamentStarted = false;

            var tournaments = this.unitOfWork.Tournaments.Get(TournamentState.Open);

            foreach(var tournament in tournaments)
            {
                if (tournament.CanStart)
                {
                    Log.Info().Message("Starting tournament {0}", tournament.Id);

                    tournament.Start();
                    tournamentStarted = true;

                    Log.Info().Message("Started.");
                }
            }

            return tournamentStarted;
        }

        public void CheckTournaments()
        {
            var tournaments = this.unitOfWork.Tournaments.Get(Tournament.ActiveStates);

            foreach(var tournament in tournaments)
            {
                try
                {
                    Log.Info().Message("Checking tournament {0}", tournament.Id).Write();

                    this.SynchronizeGamesToPairings(tournament);

                    if (tournament.CanStartNextRound)
                    {
                        tournament.StartNextRound();
                    }
                    else if (tournament.CanEnd)
                    {
                        tournament.End();
                    }

                    this.CreateGamesForPairings(tournament);
                }
                catch (Exception ex)
                {
                    Log.Error().Message("Error while handling tournament {0}", tournament.Id).Exception(ex).Write();
                }
            }
        }

        public void CreateGamesForPairings(Tournament tournament)
        {
            foreach(var pairing in tournament.Pairings.Where(
                x => x.State == PairingState.None && x.Games.Count() != x.NumberOfGames))
            {
                this.CreateGamesForPairing(pairing);
            }
        }
        
        public void SynchronizeGamesToPairings(Tournament tournament)
        {
            var activePairings = tournament.Pairings.Where(x => x.State == PairingState.Active).ToArray();

            Log.Info().Message("Checking {0} active pairings", activePairings.Length).Write();

            foreach (var pairing in activePairings)
            {
                // Synchronize number of won games
                pairing.TeamAWon = this.CountWonGamesForTeam(pairing.Games, pairing.TeamA);
                pairing.TeamBWon = this.CountWonGamesForTeam(pairing.Games, pairing.TeamB);

                Log.Info().Message("Wins: TeamA {0} TeamB {1}", pairing.TeamAWon, pairing.TeamBWon).Write();

                if (pairing.CanWinnerBeDetermined)
                {
                    Log.Info().Message("Found winner for pairing {0}", pairing.Id).Write();

                    pairing.State = PairingState.Done;
                    pairing.Loser.State = TournamentTeamState.InActive;

                    // TODO: Generate domain event for winner
                    // TODO: Generate domain event for loser
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

        private void CreateGamesForPairing(TournamentPairing pairing)
        {
            Debug.Assert(pairing.State == PairingState.None, "Pairing state is not correct");

            var systemUser = this.unitOfWork.Users.FindByName("System");

            for (int i = 0; i < pairing.NumberOfGames; ++i)
            {
                var game = gameService.Create(
                    Enums.GameType.Tournament,
                    systemUser,
                    pairing.GenerateGameName(i),
                    pairing.Tournament.GetMapTemplateForGame(),
                    pairing.Tournament.Options);

                game.TournamentPairingId = pairing.Id;
                pairing.Games.Add(game);

                var teamA = game.AddTeam();
                foreach (var participant in pairing.TeamA.Participants)
                {
                    teamA.AddPlayer(participant.User);
                }

                var teamB = game.AddTeam();
                foreach (var participant in pairing.TeamB.Participants)
                {
                    teamB.AddPlayer(participant.User);
                }

                game.Start(this.mapTemplateProvider.GetTemplate(game.MapTemplateName));
            }

            pairing.State = PairingState.Active;
        }
    }
}
