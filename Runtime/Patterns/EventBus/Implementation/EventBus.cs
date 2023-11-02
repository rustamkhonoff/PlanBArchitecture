using System;
using System.Collections.Generic;
using System.Linq;
using Patterns.EventBus.Interfaces;
using UnityEngine;

namespace Patterns.EventBus.Implementation
{
    public class EventBus : IEventBus
    {
        private readonly Dictionary<Type, List<EventHandlerWrapper>> eventHandlers = new();

        public void Subscribe<TEvent>(Action<TEvent> handler, Func<bool> condition, int priority) where TEvent : ISignal
        {
            Type eventType = typeof(TEvent);
            if (!eventHandlers.ContainsKey(eventType))
                eventHandlers[eventType] = new List<EventHandlerWrapper>();
            
            eventHandlers[eventType].Add(new EventHandlerWrapper(handler, condition, priority));
            eventHandlers[eventType] = eventHandlers[eventType].OrderByDescending(a => a.Priority).ToList();
        }

        public void Subscribe<TEvent>(Action<TEvent> handler, Func<bool> condition) where TEvent : ISignal
        {
            Subscribe(handler, condition, 0);
        }

        public void Subscribe<TEvent>(Action<TEvent> handler, int priority) where TEvent : ISignal
        {
            Subscribe(handler, null, 0);
        }

        public void Unsubscribe<TEvent>(Action<TEvent> handler) where TEvent : ISignal
        {
            Type eventType = typeof(TEvent);
            if (eventHandlers.ContainsKey(eventType))
                eventHandlers[eventType].RemoveAll(wrapper => wrapper.Handler as Action<TEvent> == handler);
        }

        public void Publish<TEvent>(TEvent eventData) where TEvent : ISignal
        {
            Type eventType = typeof(TEvent);
            if (!eventHandlers.ContainsKey(eventType))
            {
                Debug.LogWarning($"There is no event handler for type {eventType.Name}");
                return;
            }

            List<EventHandlerWrapper> handlers = eventHandlers[eventType];
            foreach (EventHandlerWrapper wrapper in handlers)
            {
                if (wrapper.Handler is not Action<TEvent> castedHandler)
                    continue;

                if (wrapper.Condition != null)
                {
                    if (wrapper.Condition())
                        castedHandler.Invoke(eventData);
                }
                else
                {
                    castedHandler.Invoke(eventData);
                }
            }
        }

        private class EventHandlerWrapper
        {
            public int Priority { get; }
            public object Handler { get; }
            public Func<bool> Condition { get; }

            public EventHandlerWrapper(object handler, Func<bool> condition = null, int priority = 0)
            {
                Handler = handler;
                Condition = condition;
                Priority = priority;
            }
        }
    }
}