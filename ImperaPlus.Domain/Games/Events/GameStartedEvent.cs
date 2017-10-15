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
