using System;

namespace DependencyInjectionService
{
    //Only use with methods to keep the same style in using DI service
    [AttributeUsage(AttributeTargets.Method)]
    public class Injectable :
#if ZENJECT
        Zenject.InjectAttribute
#endif
    {
    }
    
    //For those who somehow want to keep injecting dependencies into anything
    public class InjectableAny :
#if ZENJECT
        Zenject.InjectAttribute
#endif
    {
    }
}