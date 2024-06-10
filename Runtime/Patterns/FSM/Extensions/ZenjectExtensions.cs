#if ZENJECT
using System;
using Patterns.FSM.Implementations;
using Zenject;

namespace Patterns.FSM.Extensions
{
    public static class ZenjectExtensions
    {
        public static void AddGlobalStateMachine(this DiContainer diContainer, Type stateType = null, object transfer = null)
        {
            diContainer
                .Bind<IStateFactory>()
                .To<DiStateFactory>()
                .AsSingle();

            diContainer
                .BindInterfacesAndSelfTo<StateMachine>()
                .AsSingle()
                .WithArguments(stateType, transfer);
        }

        public static void AddGlobalStateMachine<TState>(this DiContainer diContainer) where TState : State
        {
            AddGlobalStateMachine(diContainer, typeof(TState));
        }

        public static void AddGlobalStateMachine<TState, TTransfer>(this DiContainer diContainer, TTransfer transfer)
            where TState : TransferState<TTransfer>
        {
            AddGlobalStateMachine(diContainer, typeof(TState), transfer);
        }
    }
}
#endif