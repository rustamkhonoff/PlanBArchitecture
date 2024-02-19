using System;
using System.Collections.Generic;
using Services.UI.Window;

namespace Services.UI.Interfaces
{
    public interface IUIService
    {
        TWindow CreateWindowOfType<TWindow>(bool onlyOneInstance = true)
            where TWindow : Window.Window;

        ModelWindow<TModel> CreateWindowOfType<TWindow, TModel>(TModel model, bool onlyOneInstance = true)
            where TWindow : ModelWindow<TModel>;

        ModelWindow<TModel> CreateWindowForModel<TModel>(TModel model, bool onlyOneInstance = true);
        bool TryGetActiveWindows(out IList<WindowBase> instances, Type type);
        void DestroyWindowsOfModelType<TModel>();
        void DestroyWindowOfType<TWindow>() where TWindow : WindowBase;
        void DestroyAll();
        bool IsWindowActive<TWindow>() where TWindow : WindowBase;
        bool IsWindowForModelActive<TModel>();
    }
}