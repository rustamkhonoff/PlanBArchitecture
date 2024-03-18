using System;
using DependencyInjectionService;

namespace FSM.Implementations
{
    internal class DiStateFactory : IStateFactory
    {
        private readonly IDependencyInjectionService m_dependencyInjectionService;

        public DiStateFactory(IDependencyInjectionService dependencyInjectionService)
        {
            m_dependencyInjectionService = dependencyInjectionService;
        }

        public object CreateState(Type type)
        {
            return m_dependencyInjectionService.Create(type);
        }
    }
}