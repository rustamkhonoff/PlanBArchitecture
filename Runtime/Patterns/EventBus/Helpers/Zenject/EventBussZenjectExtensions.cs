using Patterns.EventBus.Interfaces;
using Zenject;

namespace Patterns.EventBus.Helpers.Zenject
{
    public static class EventBussZenjectExtensions
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