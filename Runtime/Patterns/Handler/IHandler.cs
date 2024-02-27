using System;

namespace Handler
{
    public interface IHandler<T>
    {
        void Handle(T handleObject);
        void SetNextHandle(IHandler<T> next);
        void SetFinalHandlerAction(Action<T> finalAction);
    }
}