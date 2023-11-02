using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Patterns.Mediator.Interfaces;
using Patterns.Mediator.Publisher;
using Patterns.Mediator.Sender;

namespace Patterns.Mediator.Implementation
{
    public class Mediator : IMediator
    {
        private readonly IMediatorTypeFactory m_mediatorTypeFactory;
        private readonly Dictionary<Type, Type> m_mappedNotificationHandlers;
        private readonly Dictionary<Type, IList> m_cachedNotificationHandlerInstances;

        private readonly Dictionary<Type, Type> m_mappedRequestHandlers;
        private readonly Dictionary<Type, object> m_cachedRequestHandlerInstances;

        public Mediator(IEnumerable<Assembly> assemblies, IMediatorTypeFactory mediatorTypeFactory)
        {
            m_mediatorTypeFactory = mediatorTypeFactory;

            m_cachedNotificationHandlerInstances = new Dictionary<Type, IList>();
            m_mappedRequestHandlers = new Dictionary<Type, Type>();

            m_mappedNotificationHandlers = new Dictionary<Type, Type>();
            m_cachedRequestHandlerInstances = new Dictionary<Type, object>();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    Type[] allGenericInterfaces = type
                        .GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .ToArray();

                    Type[] notificationHandlerTypes = allGenericInterfaces
                        .Where(a => a.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
                        .SelectMany(a => a.GetGenericArguments())
                        .ToArray();
                    foreach (Type item in notificationHandlerTypes)
                        m_mappedNotificationHandlers.Add(type, item);

                    Type[] requestHandlerTypes = allGenericInterfaces
                        .Where(a => a.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                        .ToArray();
                    foreach (Type item in requestHandlerTypes)
                        m_mappedRequestHandlers.Add(item.GetGenericArguments()[0], type);
                }
            }
        }

        public void Publish<T>(T notification) where T : INotification
        {
            Type notificationType = notification.GetType();

            if (!m_cachedNotificationHandlerInstances.TryGetValue(notificationType, out IList handlers))
            {
                IList newHandlersList = m_mappedNotificationHandlers
                    .Where(a => a.Value == notificationType)
                    .Select(a => CreateInstanceOf(a.Key))
                    .ToList();
                m_cachedNotificationHandlerInstances.Add(notificationType, newHandlersList);
                handlers = newHandlersList;
            }

            if (handlers == null)
                throw new NullReferenceException($"Can' create handlers for {notificationType}");

            PublishBase(notification, handlers);
        }

        private void PublishBase<T>(T input, IEnumerable handlers) where T : INotification
        {
            foreach (INotificationHandler<T> handler in handlers.OfType<INotificationHandler<T>>())
                handler.Handle(input);
        }

        public T Send<T>(IRequest<T> request)
        {
            Type requestType = request.GetType();

            Type requiredType = requestType
                .GetInterfaces()
                .First(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IRequest<>))
                .GetGenericArguments()
                .First();

            if (!m_cachedRequestHandlerInstances.TryGetValue(requestType, out object handler))
            {
                if (m_mappedRequestHandlers.TryGetValue(requestType, out Type handlerType))
                {
                    object newHandlerInstance = CreateInstanceOf(handlerType);
                    m_cachedRequestHandlerInstances.Add(requestType, newHandlerInstance);
                    handler = newHandlerInstance;
                }
            }

            if (handler != null)
                return SendBase(handler, request, requestType, requiredType);

            throw new NullReferenceException($"Can't create handler for {requestType}");
        }

        private T SendBase<T>(object instance, IRequest<T> request, Type requestType, Type requiredType)
        {
            Type requestHandlerType = typeof(IRequestHandler<,>);
            Type genericRequestHandler = requestHandlerType.MakeGenericType(requestType, requiredType);
            MethodInfo method = genericRequestHandler.GetMethod("Process");
            object result = method?.Invoke(instance, new object[] { request });
            return (T)result;
        }

        private object CreateInstanceOf(Type type)
        {
            return m_mediatorTypeFactory.CreateInstanceFor(type);
        }
    }
}