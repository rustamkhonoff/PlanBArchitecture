#if ZENJECT
using Zenject;

namespace Services.DependencyInjectionService.Implementation.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddDependencyInjection(this DiContainer diContainer)
        {
            diContainer
                .BindInterfacesAndSelfTo<ZenjectDependencyInjection>()
                .AsSingle();
        }
    }
}
#endif