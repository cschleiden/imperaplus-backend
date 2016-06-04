using Autofac;
using System;
using System.Collections.Generic;

namespace ImperaPlus.Domain.Events
{   
    public interface IEventHandler<TEvent> 
        where TEvent : IDomainEvent
    {
        void Handle(TEvent evt);
    }    
}
