#if ZENJECT
using Patterns.EventBus.Interfaces;
using Zenject;

namespace Patterns.EventBus.Extensions.Zenject
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