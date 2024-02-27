using System;

namespace Mediator
{
    public interface IMediatorTypeFactory
    {
        object CreateInstanceFor(Type type);
    }
}