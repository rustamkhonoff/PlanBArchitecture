namespace Patterns.Mediator.Implementation
{
    public abstract class RequestHandlerBase
    {
        public abstract object Handle(object request, object handler);
    }
}