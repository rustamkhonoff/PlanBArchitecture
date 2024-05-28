using System;

namespace DependencyInjectionService
{
    //Only use with methods to keep the same style in using DI service
    [AttributeUsage(AttributeTargets.Method)] [Obsolete("Use DI attribute instead")]
    public class Injectable :
#if ZENJECT
        Zenject.InjectAttribute
#else
    Attribute
#endif
    { }

    [AttributeUsage(AttributeTargets.Method)]
    // ReSharper disable once InconsistentNaming
    public class DI : Injectable { }

    //For those who somehow want to keep injecting dependencies into anything
    [Obsolete("Use DIAny attribute instead")]
    public class InjectableAny :
#if ZENJECT
        Zenject.InjectAttribute
#else
    Attribute
#endif
    { }

    public class DIAny : InjectableAny { }
}