using System;

namespace Mediator
{
    internal sealed class ActivatorMediatorTypeFactory : IMediatorTypeFactory
    {
        public object CreateInstanceFor(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}