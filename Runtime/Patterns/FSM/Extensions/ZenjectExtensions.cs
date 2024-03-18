#if ZENJECT
using System;
using Zenject;

namespace FSM.Extensions
{
    public static class ZenjectExtensions
    {
        public static void AddGlobalStateMachine(this DiContainer diContainer, Type stateType = null, object transfer = null)
        {
            diContainer
                .BindInterfacesAndSelfTo<StateMachine>()
                .AsSingle()
                .WithArguments(stateType, transfer);
        }

        public static void AddGlobalStateMachine<TState>(this DiContainer diContainer) where TState : State
        {
            diContainer
                .BindInterfacesAndSelfTo<StateMachine>()
                .AsSingle()
                .WithArguments(typeof(TState));
        }

        public static void AddGlobalStateMachine<TState, TTransfer>(this DiContainer diContainer, TTransfer transfer)
            where TState : TransferState<TTransfer>
        {
            diContainer
                .BindInterfacesAndSelfTo<StateMachine>()
                .AsSingle()
                .WithArguments(typeof(TState), transfer);
        }
    }
}
#endif