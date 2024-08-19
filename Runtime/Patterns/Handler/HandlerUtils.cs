using System;
using System.Collections.Generic;
using System.Linq;

namespace Patterns.Handler
{
    public static class HandlerUtils
    {
        public static void CreateHandlersLinkAndRun<T>(this IEnumerable<Type> types, Func<Type, object> createFunc,
            T handleObject,
            Action<T> finalAction = null)
        {
            CreateHandlers<T>(createFunc, types).LinkAndRunHandlers(handleObject, finalAction);
        }

        public static void LinkAndRunHandlers<T>(this IEnumerable<IHandler<T>> handlers, T handleObject,
            Action<T> finalAction = null)
        {
            handlers.LinkHandlers(finalAction).First().Handle(handleObject);
        }

        public static IEnumerable<IHandler<T>> CreateAndLinkHandlers<T>(this IEnumerable<Type> types,
            Func<Type, object> createFunc, Action<T> finalAction = null)
        {
            var handlers = CreateHandlers<T>(createFunc, types);
            return handlers.LinkHandlers(finalAction);
        }

        public static IEnumerable<IHandler<T>> CreateHandlers<T>(Func<Type, object> createFunc, IEnumerable<Type> types)
        {
            List<IHandler<T>> handlerInstances =
                types
                    .Where(type => IsImplementsInterface(type, typeof(IHandler<>)))
                    .Select(type => createFunc.Invoke(type) as IHandler<T>)
                    .ToList();
            return handlerInstances;
        }

        public static List<IHandler<T>> LinkHandlers<T>(this IEnumerable<IHandler<T>> handlers,
            Action<T> finalAction = null)
        {
            List<IHandler<T>> handlerList = new();
            IHandler<T> previous = null;

            foreach (IHandler<T> handler in handlers)
            {
                if (previous != null)
                    previous.SetNextHandle(handler);
                previous = handler;
                handlerList.Add(handler);
            }

            previous?.SetFinalHandlerAction(finalAction);
            return handlerList;
        }


        private static bool IsImplementsInterface(Type toCheck, Type implementation)
        {
            return toCheck
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == implementation);
        }
    }
}