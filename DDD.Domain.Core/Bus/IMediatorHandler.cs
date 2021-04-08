using DDD.Domain.Core.Commands;
using DDD.Domain.Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Core.Bus
{
    public interface IMediatorHandler
    {
        Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event;

        Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken));
    }
}
