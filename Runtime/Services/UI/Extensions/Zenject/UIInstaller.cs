#if ZENJECT
using UI.Implementation;
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