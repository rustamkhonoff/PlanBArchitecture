using System;
using UnityEngine;

namespace UI
{
    public abstract class UIElement : MonoBehaviour
    {
        public event Action Destroying;

        public virtual void Initialize()
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual void Hide(Action onHided = default)
        {
            gameObject.SetActive(false);
            onHided?.Invoke();
        }

        public void HideAndDestroy()
        {
            Hide(DestroySelf);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            Destroying?.Invoke();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}