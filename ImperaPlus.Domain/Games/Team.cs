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
            this.Id = Guid.NewGuid();
        }

        public Team(Game game)
            : this()
        {
            this.Players = new HashSet<Player>();
            this.Game = game;
            this.PlayOrder = game.Teams.Count();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public virtual ICollection<Player> Players { get; private set; }
       
        public long GameId { get; set; }
        public Game Game { get; set; }
        
        public int PlayOrder { get; set; }

        /// <summary>
        /// Aggregation of player's outcome
        /// </summary>
        public PlayerOutcome Outcome
        {
            get
            {
                if (this.Players.All(x => x.Outcome == PlayerOutcome.Won))
                {
                    return PlayerOutcome.Won;
                }

                if (this.Players.Any(x => x.Outcome == PlayerOutcome.None))
                {
                    return PlayerOutcome.None;
                }

                return PlayerOutcome.Defeated;
            }
        }

        public Player AddPlayer(User user)
        {
            Require.NotNull(user, "user");

            if (this.Players.Count() >= this.Game.Options.NumberOfPlayersPerTeam)
            {
                throw new DomainException(ErrorCode.TeamAlreadyFull, "Team is already full");
            }

            var player = new Player(this.Game, user, this);

            this.Players.Add(player);

            return player;
        }

        internal void RemovePlayer(Player player)
        {
            Require.NotNull(player, "player");

            this.Players.Remove(player);
        }
    }
}
