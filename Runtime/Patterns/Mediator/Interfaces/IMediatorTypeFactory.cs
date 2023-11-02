using System;

namespace Patterns.Mediator.Interfaces
{
    public interface IMediatorTypeFactory
    {
        object CreateInstanceFor(Type type);
    }
}