namespace Mediator
{
    public interface IPublisher
    {
        void Publish<T>(T notification) where T : INotification;
    }
}