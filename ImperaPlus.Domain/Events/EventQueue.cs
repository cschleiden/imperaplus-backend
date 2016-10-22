using System.Collections.Generic;

namespace ImperaPlus.Domain.Events
{
    public class EventQueue
    {
        public List<IDomainEvent> Events { get; } = new List<IDomainEvent>();

        public void Raise<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
        {
            this.Events.Add(domainEvent);
        }
    }
}
