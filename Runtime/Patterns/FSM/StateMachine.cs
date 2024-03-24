using System;
using System.Linq;
using System.Reflection;
using DependencyInjectionService;
using Extensions;

namespace FSM
{
    public sealed class StateMachine : ITick, IBootable
    {
        public event Action<StateBase, StateBase> StateChangedFromTo;
        public event Action<StateBase> StateChangedTo;

        private readonly IStateFactory m_stateFactory;
        private readonly Type m_defaultState;
        private readonly object m_transfer;
        private StateBase CurrentState { get; set; }

        public StateMachine(IStateFactory stateFactory, Type defaultState = null, object transfer = null)
        {
            m_stateFactory = stateFactory;
            m_defaultState = defaultState;
            m_transfer = transfer;
        }

        public void Boot()
        {
            if (m_defaultState == null)
                return;
            if (m_transfer != null)
                Enter(m_defaultState, m_transfer);
            else
                Enter(m_defaultState);
        }

        public void Enter<TState>() where TState : State
        {
            Enter(typeof(TState));
        }

        public void Enter(Type stateType)
        {
            if (!stateType.IsSubclassOf(typeof(State)))
                return;
            CurrentState?.Exit();

            State stateInstance = m_stateFactory.CreateState(stateType) as State;

            OnStateChangedFromTo(CurrentState, stateInstance);
            OnStateChangedTo(CurrentState);

            CurrentState = stateInstance;

            stateInstance?.Enter();
        }

        public void Enter<TState, TTransfer>(TTransfer transfer) where TState : TransferState<TTransfer>
        {
            Enter(typeof(TState), transfer);
        }

        public void Enter(Type stateType, object transfer)
        {
            if (!stateType.IsInheritsGenericClass(typeof(TransferState<>)))
                throw new InvalidCastException($"Given type {stateType} not inherits class TransferState");

            Type baseGenericType = stateType.BaseType;
            while (baseGenericType is { IsGenericType: false })
                baseGenericType = baseGenericType.BaseType;

            if (baseGenericType == null)
                throw new InvalidOperationException("No generic base type found for the given stateType.");

            Type transferType = baseGenericType.GetGenericArguments().First();
            if (transfer.GetType() != transferType)
                throw new InvalidCastException(
                    $"Given state transfer type is {transfer.GetType()}, expected {transferType}");

            CurrentState?.Exit();

            object stateInstance = m_stateFactory.CreateState(stateType);
            
            OnStateChangedFromTo(CurrentState, stateInstance as State);
            OnStateChangedTo(CurrentState);

            CurrentState = stateInstance as State;
            MethodInfo method = stateType.GetMethod("Enter");
            if (method == null)
                throw new InvalidCastException("There is no method with name Enter");

            method.Invoke(stateInstance, new[] { transfer });
        }

        public void Tick()
        {
            CurrentState?.Tick();
        }

        private void OnStateChangedFromTo(StateBase arg1, StateBase arg2)
        {
            StateChangedFromTo?.Invoke(arg1, arg2);
        }

        private void OnStateChangedTo(StateBase obj)
        {
            StateChangedTo?.Invoke(obj);
        }
    }
}