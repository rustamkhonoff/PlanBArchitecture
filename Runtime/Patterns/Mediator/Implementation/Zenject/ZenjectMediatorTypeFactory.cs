#if ZENJECT
using System;
using Zenject;

namespace Mediator
{
    internal sealed class ZenjectMediatorTypeFactory : IMediatorTypeFactory
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
#endif