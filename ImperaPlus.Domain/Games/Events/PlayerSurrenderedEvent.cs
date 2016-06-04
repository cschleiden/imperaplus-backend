using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Games.Events
{
    public class PlayerSurrenderedEvent : GameEvent
    {
        public PlayerSurrenderedEvent(Game game, Player player)
            : base(game)
        {
            this.Player = player;
        }

        public Player Player { get; set; }
    }
}
