using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Utilities;

namespace ImperaPlus.Domain.Tournaments
{
    public class TournamentPairing
    {
        protected TournamentPairing()
        {
            Id = Guid.NewGuid();

            Games = new HashSet<Game>();
        }

        public TournamentPairing(Tournament tournament, int phase, int order, TournamentTeam teamA,
            TournamentTeam teamB, int numberOfGames)
            : this()
        {
            Require.NotNull(teamA, nameof(teamA));
            Require.NotNull(teamB, nameof(teamB));

            Debug.Assert(numberOfGames % 2 != 0, "NumberOfGames has to be odd");

            Tournament = tournament;
            TournamentId = tournament.Id;

            Phase = phase;
            Order = order;

            TeamA = teamA;
            TeamAId = teamA.Id;

            TeamB = teamB;
            TeamBId = teamB.Id;

            NumberOfGames = numberOfGames;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public virtual TournamentTeam TeamB { get; set; }
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
                Debug.Assert(CanWinnerBeDetermined);

                return TeamAWon > TeamBWon ? TeamA : TeamB;
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
                Debug.Assert(CanWinnerBeDetermined);

                return TeamAWon < TeamBWon ? TeamA : TeamB;
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
                var requiredNumberOfWins = Math.Ceiling(NumberOfGames / 2.0);
                return TeamAWon >= requiredNumberOfWins
                       || TeamBWon >= requiredNumberOfWins;
            }
        }

        /// <summary>
        /// Generate name of game for this pairing
        /// </summary>
        /// <param name="index">Index of game</param>
        public string GenerateGameName(int index)
        {
            return $"{Tournament.Name}-{TeamA.Name}-{TeamB.Name}-{Phase}-{index}";
        }
    }
}
