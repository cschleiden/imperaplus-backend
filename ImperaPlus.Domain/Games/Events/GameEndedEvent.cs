using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Games.Events
{    
    public class GameEndedEvent : GameEvent
    {
        public GameEndedEvent(Game game)
            : base(game)
        {            
        }
    }
}
