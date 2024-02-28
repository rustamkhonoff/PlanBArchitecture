#if ZENJECT
using LocalizationService.Unity;
using LocalizationService.Unity.RuntimeLocalization;
using Zenject;

namespace LocalizationService.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUnityLocalizationService(this DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<UnityLocalizationChangeNotifier>()
                .AsSingle();

            diContainer
                .BindInterfacesAndSelfTo<UnityLocalizationService>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif