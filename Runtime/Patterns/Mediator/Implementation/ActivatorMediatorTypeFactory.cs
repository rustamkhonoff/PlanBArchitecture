using System;
using Patterns.Mediator.Interfaces;

namespace Patterns.Mediator.Implementation
{
    public sealed class ActivatorMediatorTypeFactory : IMediatorTypeFactory
    {
        public object CreateInstanceFor(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}