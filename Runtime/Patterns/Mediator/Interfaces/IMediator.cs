using Patterns.Mediator.Publisher;
using Patterns.Mediator.Sender;

namespace Patterns.Mediator.Interfaces
{
    public interface IMediator : IPublisher, ISender
    {
    }
}