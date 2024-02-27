#if ZENJECT
using System;
using System.Linq;
using Zenject;

namespace Mediator.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddMediator(this DiContainer diContainer)
        {
            diContainer.Bind<IMediator>()
                .To<Mediator>()
                .AsSingle()
                .WithArguments(typeof(Mediator).Assembly)
                .NonLazy();

            diContainer.Bind<IMediatorTypeFactory>()
                .To<ZenjectMediatorTypeFactory>()
                .AsSingle();
        }

        public static void AddMediator(this DiContainer diContainer, params Type[] types)
        {
            diContainer.Bind<IMediator>()
                .To<Mediator>()
                .AsSingle()
                .WithArguments(types.Select(a => a.Assembly).ToArray())
                .NonLazy();

            diContainer.Bind<IMediatorTypeFactory>()
                .To<ZenjectMediatorTypeFactory>()
                .AsSingle();
        }
    }
}
#endif