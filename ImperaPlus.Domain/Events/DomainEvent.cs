using Autofac;
using System.Collections.Generic;
using ImperaPlus.Utils;
using System;
using NLog.Fluent;

namespace ImperaPlus.Domain.Events
{
    public interface IEventAggregator
    {
        void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent;

        void HandleQueuedEvents();
    }

    public class EventAggregator : IEventAggregator
    {
        private readonly IComponentContext context;

        private List<IDomainEvent> queuedEvents = new List<IDomainEvent>();

        public EventAggregator(IComponentContext context)
        {
            this.context = context;
        }

        public void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent
        {
            TraceContext.Trace(
                "EventAggregator: Raise event " + args.GetType().Name,
                () =>
                {
                    this.RaiseInternal<TEvent>(args);

                    var completedHandlerType = typeof(ICompletedEventHandler<>).MakeGenericType(args.GetType());
                    if (this.context.IsRegistered(completedHandlerType))
                    {
                        // Queue for later
                        this.queuedEvents.Add(args);
                    }
                });
        }

        public void HandleQueuedEvents()
        {
            var events = this.queuedEvents.ToArray();
            this.queuedEvents.Clear();

            foreach (var queuedEvent in events)
            {
                var type = queuedEvent.GetType();

                TraceContext.Trace(
                    "EventAggregator: Handle queued event " + type.Name,
                    () =>
                    {
                        this.GetType()
                            .GetMethod("RaiseInternal")
                            .MakeGenericMethod(type)
                            .Invoke(this, new object[] { queuedEvent, true });
                    });
            }
        }

        public void RaiseInternal<TEvent>(TEvent args, bool completed = false) where TEvent : IDomainEvent
        {
            TraceContext.Trace(
            "EventAggregator: Raise internal for " + typeof(TEvent).Name,
                () =>
                {
                    IEnumerable<IEventHandler<TEvent>> handlers;
                    if (completed)
                    {
                        handlers = this.context.Resolve<IEnumerable<ICompletedEventHandler<TEvent>>>();
                    }
                    else
                    {
                        handlers = this.context.Resolve<IEnumerable<IEventHandler<TEvent>>>();
                    }

                    foreach (var handler in handlers)
                    {
                        var eventHandlerName = handler.GetType().Name;
                        using (TraceContext.Trace("EventAggregator: Handle " + eventHandlerName))
                        {
                            try
                            {
                                handler.Handle(args);
                            }
                            catch(Exception ex)
                            {
                                Log
                                    .Error()
                                    .Message("Error while handlging event {0}", eventHandlerName)
                                    .Exception(ex)
                                    .Write();
                            }
                        }
                    }
                });
        }
    }
}
