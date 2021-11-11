using ImperaPlus.Domain.Games;
using ImperaPlus.Domain.Games.Events;

namespace ImperaPlus.Domain.Ladders.Events
{
    public class LadderGameStartedEvent : GameEvent
    {
        public LadderGameStartedEvent(Ladder ladder, Game game)
            : base(game)
        {
            Ladder = ladder;
        }

        public Ladder Ladder { get; set; }
    }
}
