using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionService;
using Services.UI.Core;
using Services.UI.Interfaces;
using Services.UI.StaticData;
using Services.UI.Window;
using UnityEngine;

namespace Services.UI.Extensions.Zenject.Implementation
{
    internal class UIService : IUIService
    {
        private readonly IDependencyInjectionService m_dependencyInjectionService;
        private readonly Dictionary<Type, WindowBase> m_cachedWindows;
        private readonly Dictionary<Type, WindowBase> m_cachedModelWindows;
        private readonly List<WindowBase> m_activeWindows;
        private readonly UIStaticData m_uiStaticData;

        private UIRoot m_uiRoot;

        public UIService(IDependencyInjectionService dependencyInjectionService)
        {
            m_dependencyInjectionService = dependencyInjectionService;
            m_uiStaticData = Resources.Load<UIStaticData>("StaticData/UI/UIStaticData");
            if (m_uiStaticData == null)
                throw new NullReferenceException(nameof(UIStaticData));

            m_cachedModelWindows = new Dictionary<Type, WindowBase>();
            m_cachedWindows = new Dictionary<Type, WindowBase>();

            foreach (WindowBase window in m_uiStaticData.Windows)
            {
                Type type = window.GetType();
                if (IsModelWindow(type, out Type modelType))
                {
                    if (!m_cachedModelWindows.TryAdd(modelType, window))
                        Debug.LogWarning($"There is already window for model {modelType}");
                }
                else
                {
                    if (!m_cachedWindows.TryAdd(type, window))
                        Debug.LogWarning($"There is already window of type {type}");
                }
            }

            m_activeWindows = new List<WindowBase>();

            Initialize();
        }


        public void Initialize()
        {
            CreateUIRoot();
        }

        #region Create

        public TWindow CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window.Window
        {
            CreateUIRoot();

            Window.Window prefab = (Window.Window)m_cachedWindows[typeof(TWindow)];
            if (prefab is null)
                throw new InvalidCastException();
            TWindow instance =
                m_dependencyInjectionService.InstantiatePrefabForComponent(prefab, m_uiRoot.RootTransform) as TWindow;
            if (instance == null)
                throw new NullReferenceException();
            instance.Initialize();
            instance.Setup();
            instance.Show();
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindows(out IList<WindowBase> foundInstances, typeof(WindowBase)))
                DestroyActiveWindows(foundInstances);

            m_activeWindows.Add(instance);
            return instance;
        }

        public ModelWindow<TModel> CreateWindowOfType<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>
        {
            return CreateWindowForModel(model, onlyOneInstance);
        }

        public ModelWindow<TModel> CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true)
        {
            CreateUIRoot();

            ModelWindow<TModel> prefab = (ModelWindow<TModel>)m_cachedModelWindows[typeof(TModel)];
            if (prefab is null)
                throw new InvalidCastException();
            ModelWindow<TModel> instance =
                m_dependencyInjectionService.InstantiatePrefabForComponent(prefab, m_uiRoot.RootTransform);
            if (instance == null)
                throw new NullReferenceException();
            instance.Initialize();
            instance.Setup(model);
            instance.Show();
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindows(out IList<WindowBase> foundInstances, instance.GetType()))
                DestroyActiveWindows(foundInstances);

            m_activeWindows.Add(instance);
            return instance;
        }

        #endregion

        #region Active Windows Logic

        public bool IsWindowActive<TWindow>() where TWindow : WindowBase
            => m_activeWindows.Exists(a => a.GetType() == typeof(TWindow));

        public bool IsWindowForModelActive<TModel>() =>
            m_activeWindows.Exists(a => a.GetType() == typeof(ModelWindow<TModel>));

        public bool TryGetActiveWindows(out IList<WindowBase> instances, Type type)
        {
            instances = new List<WindowBase>();
            instances = m_activeWindows.Where(a => a.GetType() == type).Select(a => a).ToList();
            return true;
        }

        #endregion

        #region Destroy

        public void DestroyWindowsOfModelType<TModel>()
        {
            Type type = m_cachedModelWindows[typeof(TModel)].GetType();
            TryGetActiveWindows(out IList<WindowBase> foundInstances, type);
            DestroyActiveWindows(foundInstances);
        }

        public void DestroyWindowOfType<TWindow>() where TWindow : WindowBase
        {
            Type type = m_cachedWindows[typeof(TWindow)].GetType();
            TryGetActiveWindows(out IList<WindowBase> foundInstances, type);
            DestroyActiveWindows(foundInstances);
        }

        private void DestroyActiveWindows(IList<WindowBase> windows)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                WindowBase window = windows[i];
                window.HideAndDestroy();
                m_activeWindows.Remove(window);
            }
        }

        public void DestroyAll()
        {
            for (int i = 0; i < m_activeWindows.Count; i++)
            {
                WindowBase window = m_activeWindows[i];
                window.HideAndDestroy();
            }

            m_activeWindows.Clear();
        }

        #endregion

        #region Private

        private void RemoveFromActive(WindowBase windowBase)
        {
            if (windowBase == null)
                return;
            windowBase.Dispose();
            m_activeWindows.Remove(windowBase);
            m_activeWindows.RemoveAll(a => ReferenceEquals(a, null));
        }

        private void CreateUIRoot()
        {
            if (!m_uiRoot)
                m_uiRoot = m_dependencyInjectionService.InstantiatePrefabForComponent(m_uiStaticData.RootPrefab);
        }

        private bool IsModelWindow(Type t, out Type modelType)
        {
            while (t != null)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ModelWindow<>))
                {
                    modelType = t.GetGenericArguments()[0];
                    return true;
                }

                t = t.BaseType;
            }

            modelType = null;
            return false;
        }

        #endregion
    }
}