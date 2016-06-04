namespace ImperaPlus.Domain.Events
{
    /// <summary>
    /// Handler after current domain state has been persisted
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface ICompletedEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IDomainEvent
    {
    }
}
