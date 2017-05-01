using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Services;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Tournaments
{
    public enum PairingState
    {
        None = 0,

        Active,

        Done
    }

    public class TournamentPairing
    {
        protected TournamentPairing()
        {
            this.Id = Guid.NewGuid();

            this.Games = new HashSet<Game>();
        }

        public TournamentPairing(Tournament tournament, int phase, int order, TournamentTeam teamA, TournamentTeam teamB, int numberOfGames)
            : this()
        {
            Require.NotNull(teamA, nameof(teamA));
            Require.NotNull(teamB, nameof(teamB));

            Debug.Assert(numberOfGames % 2 != 0, "NumberOfGames has to be odd");

            this.Tournament = tournament;
            this.TournamentId = tournament.Id;

            this.Phase = phase;
            this.Order = order;

            this.TeamA = teamA;
            this.TeamAId = teamA.Id;

            this.TeamB = teamB;
            this.TeamBId = teamB.Id;

            this.NumberOfGames = numberOfGames;
        }

        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }
        public virtual Tournament Tournament { get; set; }

        public int Phase { get; private set; }
        public int Order { get; private set; }

        public Guid? GroupId { get; set; }
        public virtual TournamentGroup Group { get; set; }

        public Guid TeamAId { get; set; }
        public virtual TournamentTeam TeamA { get; set; }
        public int TeamAWon { get; set; }

        public Guid TeamBId { get; set; }
        public TournamentTeam TeamB { get; set; }
        public int TeamBWon { get; set; }

        public int NumberOfGames { get; set; }

        public PairingState State { get; set; }

        public virtual ICollection<Game> Games { get; set; }

        /// <summary>
        /// Gets the current winner of this pairing (according to number of games won)
        /// </summary>
        [NotMapped]
        public TournamentTeam Winner
        {
            get
            {
                Debug.Assert(this.CanWinnerBeDetermined);

                return this.TeamAWon > this.TeamBWon ? this.TeamA : this.TeamB;
            }
        }

        /// <summary>
        /// Gets the current loser of this pairing (according to number of games won/lost)
        /// </summary>
        [NotMapped]
        public TournamentTeam Loser
        {
            get
            {
                Debug.Assert(this.CanWinnerBeDetermined);

                return this.TeamAWon < this.TeamBWon ? this.TeamA : this.TeamB;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the winner can be determined from the current state
        /// </summary>
        [NotMapped]
        public bool CanWinnerBeDetermined
        {
            get
            {
                int requiredNumberOfWins = this.NumberOfGames / 2;
                return this.TeamAWon >= requiredNumberOfWins
                    || this.TeamBWon >= requiredNumberOfWins;
            }
        }

        /// <summary>
        /// Generate name of game for this pairing
        /// </summary>
        /// <param name="index">Index of game</param>
        public string GenerateGameName(int index)
        {
            return $"{this.Tournament.Name}-{this.TeamA.Name}-{this.TeamB.Name}-{index}";
        }
    }
}
