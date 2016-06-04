using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Events;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Tournaments;

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
        private IUnitOfWork unitOfWork;
        private IEventAggregator eventAggregator;

        public TournamentService(IUnitOfWork unitOfWork, IGameService gameService, IEventAggregator eventAggregator)
        {
            this.unitOfWork = unitOfWork;
            this.gameService = gameService;
            this.eventAggregator = eventAggregator;
        }

        public bool CheckOpenTournaments()
        {
            bool tournamentStarted = false;

            var tournaments = this.unitOfWork.Tournaments.Get(TournamentState.Open);

            foreach(var tournament in tournaments)
            {
                if (tournament.CanStart)
                {
                    tournament.Start();

                    tournamentStarted = true;
                }
            }

            return tournamentStarted;
        }

        public void CheckTournaments()
        {
            var tournaments = this.unitOfWork.Tournaments.Get(Tournament.ActiveStates);

            foreach(var tournament in tournaments)
            {
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
            foreach(var pairing in tournament.Pairings.Where(x => x.State == PairingState.Active))
            {
                // Synchronize number of won games
                pairing.TeamAWon = this.CountWonGamesForTeam(pairing.Games, pairing.TeamA);
                pairing.TeamBWon = this.CountWonGamesForTeam(pairing.Games, pairing.TeamB);

                if (pairing.CanWinnerBeDetermined)
                {
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

                game.Start();
            }

            pairing.State = PairingState.Active;
        }
    }
}
