using System;

namespace EventBus.Interfaces
{
    public interface IEventBus
    {
        void Subscribe<T>(Action<T> handler, Func<bool> condition, int priority) where T : ISignal;
        void Subscribe<T>(Action<T> handler, int priority = 0) where T : ISignal;
        void Subscribe<T>(Action<T> handler, Func<bool> condition) where T : ISignal;
        void Unsubscribe<T>(Action<T> handler) where T : ISignal;
        void Publish<T>(T eventData) where T : ISignal;
    }
}