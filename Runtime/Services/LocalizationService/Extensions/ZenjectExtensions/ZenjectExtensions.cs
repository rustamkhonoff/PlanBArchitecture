#if ZENJECT
namespace LocalizationService.Extensions.ZenjectExtensions
{
    public static class ZenjectExtensions
    {
#if LOCALIZATION_USE_UNITY
        public static void AddUnityLocalizationService(this Zenject.DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<Unity.RuntimeLocalization.UnityLocalizationChangeNotifier>()
                .AsSingle();

            diContainer
                .BindInterfacesAndSelfTo<Unity.UnityLocalizationService>()
                .AsSingle()
                .NonLazy();
        }
#endif

#if ZENJECT
        public static void AddDummyLocalizationService(this Zenject.DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<DummyLocalizationService>()
                .AsSingle();

            diContainer
                .BindInterfacesAndSelfTo<DummyLocalizationChangeNotifier>()
                .AsSingle()
                .NonLazy();
        }

#endif
    }
}
#endif