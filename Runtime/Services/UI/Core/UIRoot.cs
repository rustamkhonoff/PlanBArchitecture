using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("UI.Zenject")]

namespace Services.UI.Core
{
    internal class UIRoot : MonoBehaviour
    {
        [SerializeField] private Transform _rootTransform;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Reset()
        {
            _rootTransform = transform;
        }

        public Transform RootTransform => transform;
    }
}