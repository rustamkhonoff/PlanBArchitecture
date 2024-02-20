#if ZENJECT
using Services.LocalizationService.Unity;
using Zenject;

namespace Services.LocalizationService.Implementations.Unity.Addons.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUnityLocalizationService(this DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<UnityLocalizationService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif