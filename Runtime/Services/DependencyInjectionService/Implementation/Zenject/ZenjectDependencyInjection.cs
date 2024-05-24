#if ZENJECT
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Object = UnityEngine.Object;

namespace DependencyInjectionService.Implementation.Zenject
{
    public interface IInstantiateActionWrapper
    {
        void Handle(object obj);
    }

    public class TypeInstantiateActionWrapper<T> : IInstantiateActionWrapper
    {
        public event Action<T> OnInstantiated;
        public void Handle(object obj) => OnInstantiated?.Invoke((T)obj);
    }

    internal class ZenjectDependencyInjection : IDependencyInjectionService, IInitializable, IDisposable
    {
        private readonly Dictionary<Type, IInstantiateActionWrapper> m_instantiateActionWrappers;
        private DiContainer m_diContainer;
        private readonly bool m_logsEnabled;

        public ZenjectDependencyInjection(DiContainer diContainer, bool logsEnabled = false)
        {
            m_instantiateActionWrappers = new Dictionary<Type, IInstantiateActionWrapper>();
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

        T IDependencyInjectionService.Resolve<T>()
        {
            return m_diContainer.Resolve<T>();
        }

        void IDependencyInjectionService.Inject(object o)
        {
            m_diContainer.Inject(o);
        }

        T IDependencyInjectionService.Create<T>()
        {
            return NotifyInstantiate(m_diContainer.Instantiate<T>());
        }

        object IDependencyInjectionService.Create(Type type)
        {
            return NotifyInstantiate(m_diContainer.Instantiate(type));
        }

        GameObject IDependencyInjectionService.InstantiatePrefab(GameObject prefab)
        {
            return NotifyInstantiate(m_diContainer.InstantiatePrefab(prefab));
        }

        GameObject IDependencyInjectionService.InstantiatePrefab(GameObject prefab, Transform parent)
        {
            return NotifyInstantiate(m_diContainer.InstantiatePrefab(prefab, parent));
        }

        T IDependencyInjectionService.InstantiatePrefabForComponent<T>(T prefab)
        {
            return NotifyInstantiate(m_diContainer.InstantiatePrefabForComponent<T>(prefab));
        }

        T IDependencyInjectionService.InstantiatePrefabForComponent<T>(T prefab, Transform parent)
        {
            return NotifyInstantiate(m_diContainer.InstantiatePrefabForComponent<T>(prefab, parent));
        }

        public void Update(object diContainer)
        {
            if (m_logsEnabled)
                Debug.Log("DiContainer [UPDATED]");
            m_diContainer = diContainer as DiContainer;
        }

        public void RegisterInstantiateHandler<T>(Action<T> onInstantiated)
        {
            Type type = typeof(T);
            if (m_instantiateActionWrappers.TryGetValue(type, out IInstantiateActionWrapper wrapper) &&
                wrapper as TypeInstantiateActionWrapper<T> is { } typeWrapper)
            {
                typeWrapper.OnInstantiated += onInstantiated;
            }
            else
            {
                TypeInstantiateActionWrapper<T> newWrapper = new();
                newWrapper.OnInstantiated += onInstantiated;
                m_instantiateActionWrappers.Add(type, newWrapper);
            }
        }

        public void UnRegisterInstantiateHandler<T>(Action<T> onInstantiated)
        {
            Type type = typeof(T);
            if (m_instantiateActionWrappers.TryGetValue(type, out IInstantiateActionWrapper wrapper) &&
                wrapper as TypeInstantiateActionWrapper<T> is { } typeWrapper)
            {
                typeWrapper.OnInstantiated -= onInstantiated;
            }
        }

        #endregion

        private T NotifyInstantiate<T>(T instance)
        {
            GetInstantiateWrapper<T>()?.Handle(instance);
            return instance;
        }

        private object NotifyInstantiate(object instance)
        {
            GetInstantiateWrapper(instance.GetType())?.Handle(instance);
            return instance;
        }

        private IInstantiateActionWrapper GetInstantiateWrapper(Type type)
        {
            return m_instantiateActionWrappers.TryGetValue(type, out IInstantiateActionWrapper wrapper) ? wrapper : null;
        }

        private IInstantiateActionWrapper GetInstantiateWrapper<T>()
        {
            return m_instantiateActionWrappers.TryGetValue(typeof(T), out IInstantiateActionWrapper wrapper) ? wrapper : null;
        }
    }
}
#endif