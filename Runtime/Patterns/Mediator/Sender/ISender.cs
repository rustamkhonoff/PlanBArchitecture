namespace Patterns.Mediator.Sender
{
    public interface ISender
    {
        T Send<T>(IRequest<T> request);
    }
}