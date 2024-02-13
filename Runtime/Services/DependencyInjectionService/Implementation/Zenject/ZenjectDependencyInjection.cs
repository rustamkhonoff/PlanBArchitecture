#if ZENJECT
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace Services.DependencyInjectionService.Implementation.Zenject
{
    internal class ZenjectDependencyInjection : IDependencyInjectionService, IInitializable, IDisposable
    {
        private DiContainer m_diContainer;

        public ZenjectDependencyInjection(DiContainer diContainer)
        {
            m_diContainer = diContainer;
        }

        public void Initialize()
        {
            SceneManager.activeSceneChanged += HandleSceneChange;
        }

        private void HandleSceneChange(Scene arg0, Scene arg1)
        {
            if (arg0 == arg1)
                return;
            if (Object.FindObjectOfType<SceneContext>() is { } sceneContext)
                Update(sceneContext.Container);
        }

        public void Dispose()
        {
            Debug.Log("ZenjectDependencyInjection disposed");
            SceneManager.activeSceneChanged -= HandleSceneChange;
        }

        #region Implementation

        public T Resolve<T>()
        {
            return m_diContainer.Resolve<T>();
        }

        public void Inject(object o)
        {
            m_diContainer.Inject(o);
        }

        public T Create<T>()
        {
            return m_diContainer.Instantiate<T>();
        }

        public object Create(Type type)
        {
            return m_diContainer.Instantiate(type);
        }

        public GameObject InstantiatePrefab(GameObject prefab)
        {
            return m_diContainer.InstantiatePrefab(prefab);
        }

        public T InstantiatePrefabForComponent<T>(T prefab) where T : Object
        {
            return m_diContainer.InstantiatePrefabForComponent<T>(prefab);
        }

        public T InstantiatePrefabForComponent<T>(T prefab, Transform parent) where T : Object
        {
            return m_diContainer.InstantiatePrefabForComponent<T>(prefab, parent);
        }

        public void Update(object diContainer)
        {
            Debug.Log("DiContainer [UPDATED]");
            m_diContainer = diContainer as DiContainer;
        }

        #endregion
    }
}
#endif