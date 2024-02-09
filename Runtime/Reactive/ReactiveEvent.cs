using System;
using System.Linq;

namespace Reactive
{
    [Serializable]
    public class ReactiveEvent
    {
        private event Action OnInvoke;

        public ReactiveEvent()
        {
            OnInvoke = default;
        }

        public void AddListener(Action action, bool forceSubscribe = false)
        {
            if (forceSubscribe)
                OnInvoke += action;
            else if (OnInvoke != null && !OnInvoke.GetInvocationList().Contains(action))
                OnInvoke += action;
            else if (OnInvoke == null)
                OnInvoke += action;
        }

        public void RemoveListener(Action action)
        {
            OnInvoke -= action;
        }

        public virtual void Invoke()
        {
            OnInvoke?.Invoke();
        }
    }
}