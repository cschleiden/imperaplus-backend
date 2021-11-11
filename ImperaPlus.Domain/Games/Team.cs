using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Exceptions;
using ImperaPlus.Domain.Utilities;
using System;
using ImperaPlus.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperaPlus.Domain.Games
{
    public class Team
    {
        protected Team()
        {
            Id = Guid.NewGuid();
        }

        public Team(Game game)
            : this()
        {
            Players = new HashSet<Player>();
            Game = game;
            PlayOrder = game.Teams.Count();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public virtual ICollection<Player> Players { get; private set; }

        public long GameId { get; set; }
        public virtual Game Game { get; set; }

        public int PlayOrder { get; set; }

        /// <summary>
        /// Aggregation of player's outcome
        /// </summary>
        public PlayerOutcome Outcome
        {
            get
            {
                if (Players.All(x => x.Outcome == PlayerOutcome.Won))
                {
                    return PlayerOutcome.Won;
                }

                if (Players.Any(x => x.Outcome == PlayerOutcome.None))
                {
                    return PlayerOutcome.None;
                }

                return PlayerOutcome.Defeated;
            }
        }

        public Player AddPlayer(User user)
        {
            Require.NotNull(user, "user");

            if (Players.Count() >= Game.Options.NumberOfPlayersPerTeam)
            {
                throw new DomainException(ErrorCode.TeamAlreadyFull, "Team is already full");
            }

            var player = new Player(Game, user, this);

            Players.Add(player);

            return player;
        }

        internal void RemovePlayer(Player player)
        {
            Require.NotNull(player, "player");

            Players.Remove(player);
        }
    }
}
