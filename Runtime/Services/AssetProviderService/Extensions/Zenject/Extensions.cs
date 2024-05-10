#if ZENJECT

using Zenject;

namespace Services.AssetProviderService.Extensions.Zenject
{
    public static class Extensions
    {
        public static void AddAssetProvider(this DiContainer diContainer)
        {
            diContainer
                .Bind<IAssetProvider>()
                .To<AddressableAssetProvider>()
                .AsSingle();
        }
    }
}
#endif