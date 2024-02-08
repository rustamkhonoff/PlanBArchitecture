using System;
using UnityEngine;

namespace Reactive
{
    [Serializable]
    public class ReactiveVariable<T>
    {
        [SerializeField] private T _value;

        public event Action<T> OnUpdated;

        public ReactiveVariable()
        {
        }

        public ReactiveVariable(T value)
        {
            Value = value;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (_value.Equals(value))
                    return;

                _value = value;
                OnUpdated?.Invoke(_value);
            }
        }

        public void Invoke()
        {
            OnUpdated?.Invoke(_value);
        }

        public void AddListener(Action<T> listener)
        {
            OnUpdated += listener;
        }

        public void RemoveListener(Action<T> listener)
        {
            OnUpdated -= listener;
        }
    }
}