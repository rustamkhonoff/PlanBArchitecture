using System;

namespace Patterns.FSM
{
    public interface IStateFactory
    {
        public object CreateState(Type type);
    }
}