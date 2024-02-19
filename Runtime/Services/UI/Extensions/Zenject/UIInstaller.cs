#if ZENJECT
using Services.UI.Extensions.Zenject.Implementation;
using Zenject;

namespace Services.UI.Extensions.Zenject
{
    public static class UIInstaller
    {
        public static void AddUIService(this DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<UIService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif