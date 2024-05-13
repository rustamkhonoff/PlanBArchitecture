using System;
using UnityEngine;

namespace LocalizationService
{
    public class DestroyEvent : MonoBehaviour
    {
        public event Action OnDestroying;

        public static DestroyEvent Create(GameObject gameObject, Action toDo)
        {
            if (!gameObject.TryGetComponent(out DestroyEvent destroyEvent))
                destroyEvent = gameObject.AddComponent<DestroyEvent>();//
            destroyEvent.OnDestroying += toDo;
            return destroyEvent;
        }

        private void OnDestroy()
        {
            OnDestroying?.Invoke();
        }
    }
}