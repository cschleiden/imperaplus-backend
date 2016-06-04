namespace ImperaPlus.Domain.Games.Events
{
    public class TurnEndedEvent : GameEvent
    {
        public TurnEndedEvent(Game game)
            : base(game)
        {
        }
    }
}
