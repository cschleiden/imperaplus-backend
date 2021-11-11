using System.Collections.Generic;

namespace ImperaPlus.Domain.Events
{
    public class EventQueue
    {
        public List<IDomainEvent> Events { get; } = new();

        public void Raise<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            Events.Add(domainEvent);
        }
    }
}
