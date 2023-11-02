using System;
using Patterns.Mediator.Interfaces;
using Zenject;

namespace Patterns.Mediator.Implementation
{
    public sealed class ZenjectMediatorTypeFactory : IMediatorTypeFactory
    {
        private readonly DiContainer m_diContainer;

        public ZenjectMediatorTypeFactory(DiContainer diContainer)
        {
            m_diContainer = diContainer;
        }

        public object CreateInstanceFor(Type type)
        {
            return m_diContainer.Instantiate(type);
        }
    }
}