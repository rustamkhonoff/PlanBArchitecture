#if ZENJECT
using System;
using System.Linq;
using Patterns.Mediator.Implementation.Zenject;
using Patterns.Mediator.Interfaces;
using Zenject;

namespace Patterns.Mediator.Extensions.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddMediator(this DiContainer diContainer)
        {
            diContainer.Bind<IMediator>()
                .To<Implementation.Mediator>()
                .AsSingle()
                .WithArguments(typeof(Implementation.Mediator).Assembly)
                .NonLazy();

            diContainer.Bind<IMediatorTypeFactory>()
                .To<ZenjectMediatorTypeFactory>()
                .AsSingle();
        }

        public static void AddMediator(this DiContainer diContainer, params Type[] types)
        {
            diContainer.Bind<IMediator>()
                .To<Implementation.Mediator>()
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