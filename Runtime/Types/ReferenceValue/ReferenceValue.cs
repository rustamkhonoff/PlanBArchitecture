using System;
using UnityEngine;

namespace Common.ReferenceValue
{
    [Serializable]
    public class ReferenceValue<T1, T2>
    {
        [SerializeField] private T1 _value1;
        [SerializeField] private T2 _value2;
        [SerializeField] private bool _useReference = true;

        public T1 ReferencedValue => _value1;
        public T2 OverrideValue => _value2;

        public void InvokeAction(Action<T1> referenceAction, Action<T2> overrideAction)
        {
            if (_useReference)
                referenceAction?.Invoke(_value1);
            else
                overrideAction?.Invoke(_value2);
        }

        public bool UseReference => _useReference;
    }
}