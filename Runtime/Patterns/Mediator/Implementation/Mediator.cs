using System;
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
        private readonly IMediatorTypeFactory _mediatorTypeFactory;
        private readonly Dictionary<Type, List<Type>> _notificationHandlerTypes;
        private readonly Dictionary<Type, Type> _requestHandlerTypes;
        private readonly Dictionary<Type, object> _handlerInstances = new Dictionary<Type, object>();

        public Mediator(IEnumerable<Assembly> assemblies, IMediatorTypeFactory mediatorTypeFactory)
        {
            _mediatorTypeFactory = mediatorTypeFactory;
            _notificationHandlerTypes = new Dictionary<Type, List<Type>>();
            _requestHandlerTypes = new Dictionary<Type, Type>();

            foreach (Assembly assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(INotificationHandler<>) ||
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
                            if (!_notificationHandlerTypes.ContainsKey(notificationType))
                            {
                                _notificationHandlerTypes[notificationType] = new List<Type>();
                            }

                            _notificationHandlerTypes[notificationType].Add(type);
                        }
                        else if (genericType == typeof(IRequestHandler<,>))
                        {
                            Type requestType = genericArguments[0];
                            _requestHandlerTypes[requestType] = type;
                        }
                    }
                }
            }
        }

        public void Publish<T>(T notification) where T : INotification
        {
            Type notificationType = typeof(T);
            if (!_notificationHandlerTypes.TryGetValue(notificationType, out var handlerTypes))
                return;

            foreach (object handler in handlerTypes.Select(GetHandlerInstance))
                ((INotificationHandler<T>)handler).Handle(notification);
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
        {
            Type requestType = request.GetType();
            if (!_requestHandlerTypes.TryGetValue(requestType, out Type handlerType))
                throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

            object handler = GetHandlerInstance(handlerType);
            return ((IRequestHandler<IRequest<TResponse>, TResponse>)handler).Handle(request);
        }

        private object GetHandlerInstance(Type handlerType)
        {
            if (_handlerInstances.TryGetValue(handlerType, out object handler))
                return handler;

            handler = _mediatorTypeFactory.CreateInstanceFor(handlerType);
            _handlerInstances[handlerType] = handler;

            return handler;
        }
    }
}