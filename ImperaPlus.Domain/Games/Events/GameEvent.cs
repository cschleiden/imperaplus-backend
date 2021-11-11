using ImperaPlus.Domain.Events;

namespace ImperaPlus.Domain.Games.Events
{
    public class GameEvent : IDomainEvent
    {
        private readonly Game game;

        public Game Game => game;

        public GameEvent(Game game)
        {
            this.game = game;
        }
    }
}
