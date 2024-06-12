#if ZENJECT

using Zenject;

namespace Services.AssetProviderService.Extensions.Zenject
{
    public static class Extensions
    {
#if ADDRESSABLES
        public static void AddAddressableAssetProvider(this DiContainer diContainer)
        {
            diContainer
                .Bind<IAssetProvider>()
                .To<AddressableAssetProvider>()
                .AsSingle();
        }
#endif
        public static void AddResourcesAssetProvider(this DiContainer diContainer)
        {
            diContainer
                .Bind<IAssetProvider>()
                .To<ResourcesAssetProvider>()
                .AsSingle();
        }
    }
}
#endif