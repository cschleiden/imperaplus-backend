using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Games.Events
{
    public class GameEvent : IDomainEvent
    {
        private Game game;

        public Game Game
        {
            get
            {
                return this.game;
            }
        }

        public GameEvent(Game game)
        {
            this.game = game;
        }
    }
}
