using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperaPlus.Domain.Ladders.Events
{
    public class LadderGameStartedEvent : GameEvent
    {
        public LadderGameStartedEvent(Ladder ladder, Game game)
            : base(game)
        {
            this.Ladder = ladder;
        }

        public Ladder Ladder { get; set; }
    }
}
