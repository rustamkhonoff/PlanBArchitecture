using Patterns.Mediator.Sender;

namespace Patterns.Mediator.Implementation
{
    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract TResponse Handle(IRequest<TResponse> request, object handler);
    }

    public abstract class RequestHandlerWrapper : RequestHandlerBase
    {
        public abstract Unit Handle(IRequest request, object handler);
    }
}