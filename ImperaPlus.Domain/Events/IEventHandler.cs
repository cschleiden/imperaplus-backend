namespace ImperaPlus.Domain.Events
{
    /// <summary>
    /// Handle domain event before persisting current state
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventHandler<TEvent> 
        where TEvent : IDomainEvent
    {
        void Handle(TEvent evt);
    }    
}
