using System;

namespace FSM
{
    public interface IStateFactory
    {
        public object CreateState(Type type);
    }
}