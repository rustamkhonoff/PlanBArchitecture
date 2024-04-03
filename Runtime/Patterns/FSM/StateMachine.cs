using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DependencyInjectionService;
using Extensions;
using UnityEngine;

namespace FSM
{
    public class StateChangeConditionMap : IStateChangeConditionMap
    {
        public Dictionary<(Type, Type), Func<bool>> Map { get; private set; }

        public StateChangeConditionMap(Dictionary<(Type, Type), Func<bool>> map)
        {
            Map = map;
        }

        public void Add<TFromState, TToState>(Func<bool> condition) where TFromState : StateBase where TToState : StateBase
        {
            Add(typeof(TToState), typeof(TToState), condition);
        }

        public void Add(Type from, Type to, Func<bool> condition)
        {
            Map.TryAdd((from, to), condition);
        }


        public void Remove<TFromState, TToState>() where TFromState : StateBase where TToState : StateBase
        {
            Remove(typeof(TFromState), typeof(TToState));
        }

        public void Remove(Type from, Type to)
        {
            Map.Remove((from, to));
        }
    }

    public interface IStateChangeConditionMap
    {
        public Dictionary<(Type, Type), Func<bool>> Map { get; }
    }

    public interface IStateMachine
    {
        event Action<StateBase, StateBase> StateChangedFromTo;
        event Action<StateBase> StateChangedTo;
        StateBase CurrentState { get; set; }
        void AddConditionMaps(IStateChangeConditionMap map);
        void RemoveConditionMaps(IStateChangeConditionMap map);
        void AddConditionMap(Type from, Type to, Func<bool> condition);
        void AddConditionMap<TFromState, TToState>(Func<bool> condition) where TFromState : StateBase where TToState : StateBase;
        void RemoveConditionMap(Type from, Type to);
        void RemoveConditionMap<TFromState, TToState>() where TFromState : StateBase where TToState : StateBase;
        void Enter<TState>() where TState : State;
        void Enter(Type stateType);
        void Enter<TState, TTransfer>(TTransfer transfer) where TState : TransferState<TTransfer>;
        void Enter(Type stateType, object transfer);
    }

    public sealed class StateMachine : ITick, IBootable, IStateMachine
    {
        public event Action<StateBase, StateBase> StateChangedFromTo;
        public event Action<StateBase> StateChangedTo;

        private readonly Dictionary<(Type, Type), Func<bool>> m_validTransitionsMap = new();
        private readonly IStateFactory m_stateFactory;
        private readonly Type m_defaultState;
        private readonly object m_transfer;
        public StateBase CurrentState { get; set; }

        public StateMachine(IStateFactory stateFactory, Type defaultState = null, object transfer = null)
        {
            m_stateFactory = stateFactory;
            m_defaultState = defaultState;
            m_transfer = transfer;
        }

        #region Condition

        public void AddConditionMaps(IStateChangeConditionMap map)
        {
            foreach (KeyValuePair<(Type, Type), Func<bool>> mapItem in map.Map)
                AddConditionMap(mapItem.Key.Item1, mapItem.Key.Item2, mapItem.Value);
        }

        public void RemoveConditionMaps(IStateChangeConditionMap map)
        {
            foreach (KeyValuePair<(Type, Type), Func<bool>> mapItem in map.Map)
                RemoveConditionMap(mapItem.Key.Item1, mapItem.Key.Item2);
        }

        public void AddConditionMap(Type from, Type to, Func<bool> condition)
        {
            m_validTransitionsMap.TryAdd((from, to), condition);
        }

        public void AddConditionMap<TFromState, TToState>(Func<bool> condition) where TFromState : StateBase where TToState : StateBase
        {
            AddConditionMap(typeof(TFromState), typeof(TToState), condition);
        }

        public void RemoveConditionMap(Type from, Type to)
        {
            m_validTransitionsMap.Remove((from, to));
        }

        public void RemoveConditionMap<TFromState, TToState>() where TFromState : StateBase where TToState : StateBase
        {
            RemoveConditionMap(typeof(TFromState), typeof(TToState));
        }

        private bool IsValidTransition(Type from, Type to)
        {
            if (!m_validTransitionsMap.ContainsKey((from, to)))
                return true;

            return m_validTransitionsMap[(from, to)].Invoke();
        }

        private bool IsValidTransitionToState(Type to)
        {
            if (CurrentState == null)
                return true;

            return IsValidTransition(CurrentState.GetType(), to);
        }

        #endregion

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
            if (!IsValidTransitionToState(stateType))
            {
                Debug.Log($"Can't change state to {stateType}, condition returns False");
                return;
            }

            if (!stateType.IsSubclassOf(typeof(State)))
                return;
            StateBase lastState = CurrentState;
            CurrentState?.Exit();

            State stateInstance = m_stateFactory.CreateState(stateType) as State;

            CurrentState = stateInstance;

            OnStateChangedFromTo(lastState, stateInstance);
            OnStateChangedTo(CurrentState);

            stateInstance?.Enter();
        }

        public void Enter<TState, TTransfer>(TTransfer transfer) where TState : TransferState<TTransfer>
        {
            Enter(typeof(TState), transfer);
        }

        public void Enter(Type stateType, object transfer)
        {
            if (!IsValidTransitionToState(stateType))
            {
                Debug.Log($"Can't change state to {stateType}, condition returns False");
                return;
            }

            if (!stateType.IsInheritsGenericClass(typeof(TransferState<>)))
                throw new InvalidCastException($"Given type {stateType} not inherits class TransferState");

            StateBase lastState = CurrentState;

            Type baseGenericType = stateType.BaseType;
            while (baseGenericType is { IsGenericType: false })
                baseGenericType = baseGenericType.BaseType;

            if (baseGenericType == null)
                throw new InvalidOperationException("No generic base type found for the given stateType.");

            Type transferType = baseGenericType.GetGenericArguments().First();
            if (transfer.GetType() != transferType)
                throw new InvalidCastException(
                    $"Given state transfer type is {transfer.GetType()}, expected {transferType}");

            lastState?.Exit();

            object stateInstance = m_stateFactory.CreateState(stateType);
            StateBase stateInstanceAsType = (StateBase)stateInstance;

            MethodInfo method = stateType.GetMethod("Enter");
            if (method == null)
                throw new InvalidCastException("There is no method with name Enter");

            CurrentState = stateInstanceAsType;

            OnStateChangedFromTo(lastState, CurrentState);
            OnStateChangedTo(CurrentState);

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