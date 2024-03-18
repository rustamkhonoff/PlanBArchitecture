using System;
using System.Collections.Generic;
using System.Linq;
using DependencyInjectionService;
using UI.Core;
using UI.Interfaces;
using UI.StaticData;
using UI.Window;
using UnityEngine;

namespace UI.Implementation
{
    internal class UIService : IUIService, IBootable
    {
        private readonly IDependencyInjectionService m_dependencyInjectionService;

        //All created and existing in current window instances
        private readonly List<WindowBase> m_currentCreatedWindows;

        private readonly Dictionary<Type, WindowBase> m_cachedModelWindowsTypes;
        private readonly Dictionary<Type, WindowBase> m_cachedWindowsTypes;

        private readonly UIStaticData m_uiStaticData;

        private UIRoot m_uiRoot;

        public UIService(IDependencyInjectionService dependencyInjectionService)
        {
            m_dependencyInjectionService = dependencyInjectionService;
            m_uiStaticData = Resources.Load<UIStaticData>("StaticData/UI/UIStaticData");
            if (m_uiStaticData == null)
                throw new NullReferenceException(nameof(UIStaticData));

            m_cachedModelWindowsTypes = new Dictionary<Type, WindowBase>();
            m_cachedWindowsTypes = new Dictionary<Type, WindowBase>();

            foreach (WindowBase window in m_uiStaticData.Windows)
            {
                Type type = window.GetType();
                if (IsModelWindow(type, out Type modelType))
                {
                    if (!m_cachedModelWindowsTypes.TryAdd(modelType, window))
                        Debug.LogWarning($"There is already window for model {modelType}");
                }
                else
                {
                    if (!m_cachedWindowsTypes.TryAdd(type, window))
                        Debug.LogWarning($"There is already window of type {type}");
                }
            }

            m_currentCreatedWindows = new List<WindowBase>();
        }

        public void Boot()
        {
            CheckForUIRoot();
        }

        #region Create

        ///Creates new instance of Window
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public TWindow CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window.Window
        {
            CheckForUIRoot();

            Window.Window prefab = (Window.Window)m_cachedWindowsTypes[typeof(TWindow)];
            if (prefab is null)
                throw new ArgumentNullException($"There is no Window of type {typeof(TWindow)}");
            TWindow instance =
                m_dependencyInjectionService.InstantiatePrefabForComponent(prefab, m_uiRoot.RootTransform) as TWindow;
            if (instance == null)
                throw new NullReferenceException($"Error while instantiating Window of type {typeof(TWindow)}");
            instance.Initialize();
            instance.Setup();
            instance.Show();
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindows(out IList<WindowBase> foundInstances, typeof(WindowBase)))
                DestroyActiveWindows(foundInstances);

            m_currentCreatedWindows.Add(instance);
            return instance;
        }

        ///Creates new instance of Model Window
        /// <param name="model">Required model type for window</param>
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public ModelWindow<TModel> CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true)
        {
            CheckForUIRoot();

            ModelWindow<TModel> prefab = (ModelWindow<TModel>)m_cachedModelWindowsTypes[typeof(TModel)];
            if (prefab is null)
                throw new ArgumentNullException($"There is no Window for Model with type {model.GetType()}");
            ModelWindow<TModel> instance =
                m_dependencyInjectionService.InstantiatePrefabForComponent(prefab, m_uiRoot.RootTransform);
            if (instance == null)
                throw new NullReferenceException($"Error while instantiating ModelWindow for type {model.GetType()}");
            instance.Initialize();
            instance.Setup(model);
            instance.Show();
            instance.Destroying += () => RemoveFromActive(instance);

            if (onlyOneInstance && TryGetActiveWindows(out IList<WindowBase> foundInstances, instance.GetType()))
                DestroyActiveWindows(foundInstances);

            m_currentCreatedWindows.Add(instance);
            return instance;
        }

        ///Creates new instance of Model Window
        /// <param name="model">Required model type for window</param>
        /// <param name="onlyOneInstance">If true will find and destroy other instances of same window type</param>
        public ModelWindow<TModel> CreateWindowOfType<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>
        {
            return CreateWindowForModel(model, onlyOneInstance);
        }

        #endregion

        #region Active Windows Logic

        public IList<WindowBase> GetActiveWindowsOfType<TWindow>() where TWindow : WindowBase
        {
            TryGetActiveWindows(out IList<WindowBase> instances, typeof(TWindow));
            return instances;
        }

        public IList<WindowBase> GetActiveWindowsForType<TWindow, TModel>() where TWindow : ModelWindow<TModel>
        {
            TryGetActiveWindows(out IList<WindowBase> instances, typeof(ModelWindow<TModel>));
            return instances;
        }

        public bool IsWindowActive<TWindow>() where TWindow : WindowBase
            => m_currentCreatedWindows.Exists(a => a.GetType() == typeof(TWindow));

        public bool IsWindowForModelActive<TModel>() =>
            m_currentCreatedWindows.Exists(a => a.GetType() == typeof(ModelWindow<TModel>));

        public bool TryGetActiveWindows(out IList<WindowBase> instances, Type type)
        {
            instances = m_currentCreatedWindows.Where(a => a.GetType() == type).Select(a => a).ToList();
            return true;
        }

        public void HideWindowsOfType<TWindow>() where TWindow : WindowBase
        {
            TryGetActiveWindows(out IList<WindowBase> windows, typeof(TWindow));
            foreach (WindowBase windowBase in windows)
                windowBase.Hide();
        }

        public void HideAndDestroyWindowsOfType<TWindow>() where TWindow : WindowBase
        {
            TryGetActiveWindows(out IList<WindowBase> windows, typeof(TWindow));
            foreach (WindowBase windowBase in windows)
                windowBase.HideAndDestroy();
        }

        #endregion

        #region Destroy

        public void DestroyWindowsOfModelType<TModel>()
        {
            Type type = m_cachedModelWindowsTypes[typeof(TModel)].GetType();
            TryGetActiveWindows(out IList<WindowBase> foundInstances, type);
            DestroyActiveWindows(foundInstances);
        }

        public void DestroyWindowOfType<TWindow>() where TWindow : WindowBase
        {
            Type type = m_cachedWindowsTypes[typeof(TWindow)].GetType();
            TryGetActiveWindows(out IList<WindowBase> foundInstances, type);
            DestroyActiveWindows(foundInstances);
        }

        private void DestroyActiveWindows(IList<WindowBase> windows)
        {
            for (int i = 0; i < windows.Count; i++)
            {
                WindowBase window = windows[i];
                window.HideAndDestroy();
                m_currentCreatedWindows.Remove(window);
            }
        }

        public void DestroyAll()
        {
            for (int i = 0; i < m_currentCreatedWindows.Count; i++)
            {
                WindowBase window = m_currentCreatedWindows[i];
                window.HideAndDestroy();
            }

            m_currentCreatedWindows.Clear();
        }

        #endregion

        #region Private

        private void RemoveFromActive(WindowBase windowBase)
        {
            if (windowBase == null)
                return;
            windowBase.Dispose();
            m_currentCreatedWindows.Remove(windowBase);
            m_currentCreatedWindows.RemoveAll(a => ReferenceEquals(a, null));
        }

        private void CheckForUIRoot()
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