#if ZENJECT
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace DependencyInjectionService.Implementation.Zenject
{
    internal class ZenjectDependencyInjection : IDependencyInjectionService, IInitializable, IDisposable
    {
        private DiContainer m_diContainer;
        private readonly bool m_logsEnabled;

        public ZenjectDependencyInjection(DiContainer diContainer, bool logsEnabled = false)
        {
            m_diContainer = diContainer;
            m_logsEnabled = logsEnabled;
        }

        void IInitializable.Initialize()
        {
            TryUpdateDiContainer();
            SceneManager.activeSceneChanged += HandleSceneChange;
        }

        private void HandleSceneChange(Scene arg0, Scene arg1)
        {
            if (arg0 == arg1)
                return;
            if (m_logsEnabled)
                Debug.Log("Start updating DiContainer after scene change");
            TryUpdateDiContainer();
        }

        private void TryUpdateDiContainer()
        {
            if (Object.FindObjectOfType<SceneContext>() is { } sceneContext)
                Update(sceneContext.Container);
        }

        void IDisposable.Dispose()
        {
            if (m_logsEnabled)
                Debug.Log("ZenjectDependencyInjection disposed");
            SceneManager.activeSceneChanged -= HandleSceneChange;
        }

        #region Implementation

        T IDependencyInjectionService.Resolve<T>() => m_diContainer.Resolve<T>();

        void IDependencyInjectionService.Inject(object o) => m_diContainer.Inject(o);

        T IDependencyInjectionService.Create<T>() => m_diContainer.Instantiate<T>();

        object IDependencyInjectionService.Create(Type type) => m_diContainer.Instantiate(type);

        GameObject IDependencyInjectionService.InstantiatePrefab(GameObject prefab) => m_diContainer.InstantiatePrefab(prefab);

        GameObject IDependencyInjectionService.InstantiatePrefab(GameObject prefab, Transform parent) =>
            m_diContainer.InstantiatePrefab(prefab, parent);

        T IDependencyInjectionService.InstantiatePrefabForComponent<T>(T prefab) => m_diContainer.InstantiatePrefabForComponent<T>(prefab);

        T IDependencyInjectionService.InstantiatePrefabForComponent<T>(T prefab, Transform parent) =>
            m_diContainer.InstantiatePrefabForComponent<T>(prefab, parent);

        public void Update(object diContainer)
        {
            if (m_logsEnabled)
                Debug.Log("DiContainer [UPDATED]");
            m_diContainer = diContainer as DiContainer;
        }

        #endregion
    }
}
#endif