using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Games.Events
{
    public class GameStartedEvent : GameEvent
    {
        public GameStartedEvent(Game game)
            : base(game)
        {
        }
    }
}
