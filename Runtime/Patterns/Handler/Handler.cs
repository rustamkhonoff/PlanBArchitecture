using System;

namespace Handler
{
    public abstract class Handler<T> : IHandler<T>
    {
        public abstract void Handle(T handleObject);
        public void SetNextHandle(IHandler<T> next) => NextHandler = next;

        public void SetFinalHandlerAction(Action<T> finalAction) => FinalHandlerAction = finalAction;

        protected void HandleNext(T handleObject)
        {
            if (NextHandler != null)
                NextHandler?.Handle(handleObject);
            else
                FinalHandlerAction?.Invoke(handleObject);
        }

        private IHandler<T> NextHandler { get; set; }
        private Action<T> FinalHandlerAction { get; set; }
    }
}