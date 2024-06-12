#if ZENJECT
using Zenject;

namespace DependencyInjectionService.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddDependencyInjection(this DiContainer diContainer, bool logsEnabled = false)
        {
            diContainer
                .BindInterfacesAndSelfTo<ZenjectDependencyInjection>()
                .AsSingle()
                .WithArguments(logsEnabled);
        }
    }
}

#endif