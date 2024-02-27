#if ZENJECT
using EventBus.Interfaces;
using Zenject;

namespace EventBus.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddEventBuss(this DiContainer diContainer)
        {
            diContainer
                .Bind<IEventBus>()
                .To<Implementation.EventBus>()
                .AsSingle();
        }
    }
}
#endif