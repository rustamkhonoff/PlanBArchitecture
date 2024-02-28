using System;
using System.Collections.Generic;

namespace UI
{
    public interface IUIService
    {
        TWindow CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window;

        ModelWindow<TModel> CreateWindowOfType<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>;

        ModelWindow<TModel> CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true);
        IList<WindowBase> GetActiveWindowsOfType<TWindow>() where TWindow : WindowBase;
        IList<WindowBase> GetActiveWindowsForType<TWindow, TModel>() where TWindow : ModelWindow<TModel>;
        bool TryGetActiveWindows(out IList<WindowBase> instances, Type type);
        void DestroyWindowsOfModelType<TModel>();
        void DestroyWindowOfType<TWindow>() where TWindow : WindowBase;
        void HideWindowsOfType<TWindow>() where TWindow : WindowBase;
        void HideAndDestroyWindowsOfType<TWindow>() where TWindow : WindowBase;
        void DestroyAll();
        bool IsWindowActive<TWindow>() where TWindow : WindowBase;
        bool IsWindowForModelActive<TModel>();
    }
}