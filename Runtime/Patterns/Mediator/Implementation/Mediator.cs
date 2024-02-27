using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Mediator
{
    internal class Mediator : IMediator
    {
        private readonly IMediatorTypeFactory m_typeFactory;
        private readonly Dictionary<Type, List<Type>> m_notificationHandlerTypes;
        private readonly Dictionary<Type, Type> m_requestHandlerTypes;
        private readonly Dictionary<Type, object> m_handlerInstances;
        private readonly Dictionary<Type, RequestHandlerBase> m_requestHandlers;

        public Mediator(IEnumerable<Assembly> assemblies, IMediatorTypeFactory typeFactory)
        {
            m_typeFactory = typeFactory;
            m_notificationHandlerTypes = new Dictionary<Type, List<Type>>();
            m_requestHandlerTypes = new Dictionary<Type, Type>();
            m_handlerInstances = new Dictionary<Type, object>();
            m_requestHandlers = new Dictionary<Type, RequestHandlerBase>();

            foreach (Assembly assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() ==
                     typeof(INotificationHandler<>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))));

                foreach (Type type in types)
                {
                    var handlerInterfaces = type.GetInterfaces().Where(i => i.IsGenericType);
                    foreach (Type handlerInterface in handlerInterfaces)
                    {
                        Type genericType = handlerInterface.GetGenericTypeDefinition();
                        var genericArguments = handlerInterface.GetGenericArguments();

                        if (genericType == typeof(INotificationHandler<>))
                        {
                            Type notificationType = genericArguments[0];
                            if (!m_notificationHandlerTypes.ContainsKey(notificationType))
                            {
                                m_notificationHandlerTypes[notificationType] = new List<Type>();
                            }

                            m_notificationHandlerTypes[notificationType].Add(type);
                        }
                        else if (genericType == typeof(IRequestHandler<,>) || genericType == typeof(IRequestHandler<>))
                        {
                            Type requestType = genericArguments[0];
                            if (m_requestHandlerTypes.ContainsKey(requestType))
                            {
                                Debug.LogError(
                                    $"Request handler {m_requestHandlerTypes[requestType]} for type {requestType} already defined, ignoring {type}");
                                continue;
                            }

                            m_requestHandlerTypes[requestType] = type;
                        }
                    }
                }
            }
        }

        public void Publish<T>(T notification) where T : INotification
        {
            Type notificationType = typeof(T);
            if (!m_notificationHandlerTypes.TryGetValue(notificationType, out var handlerTypes))
                return;

            foreach (object handler in handlerTypes.Select(GetOrCreateHandlerInstance))
                ((INotificationHandler<T>)handler).Handle(notification);
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            Type requestType = request.GetType();

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!m_requestHandlerTypes.TryGetValue(requestType, out Type handlerType))
                throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

            if (!m_requestHandlers.TryGetValue(requestType, out RequestHandlerBase wrapperInstance))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse));
                RequestHandlerBase wrapper = (RequestHandlerBase)Activator.CreateInstance(wrapperType);
                wrapperInstance = wrapper;
                m_requestHandlers.Add(requestType, wrapper);
            }

            object handlerInstance = GetOrCreateHandlerInstance(handlerType);

            return (TResponse)wrapperInstance.Handle(request, handlerInstance);
        }

        public void Send<T>(T request) where T : IRequest
        {
            Type requestType = request.GetType();

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!m_requestHandlerTypes.TryGetValue(requestType, out Type handlerType))
                throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

            if (!m_requestHandlers.TryGetValue(requestType, out RequestHandlerBase wrapperInstance))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<>).MakeGenericType(requestType);
                RequestHandlerBase wrapper = (RequestHandlerBase)Activator.CreateInstance(wrapperType);
                wrapperInstance = wrapper;
                m_requestHandlers.Add(requestType, wrapper);
            }

            object handlerInstance = GetOrCreateHandlerInstance(handlerType);
            wrapperInstance.Handle(request, handlerInstance);
        }

        private object GetOrCreateHandlerInstance(Type handlerType)
        {
            if (m_handlerInstances.TryGetValue(handlerType, out object handler))
                return handler;

            handler = m_typeFactory.CreateInstanceFor(handlerType);
            m_handlerInstances[handlerType] = handler;

            return handler;
        }
    }
}