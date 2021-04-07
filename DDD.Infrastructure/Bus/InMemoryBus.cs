using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Commands;
using DDD.Domain.Core.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Infrastructure.Bus
{
    public class InMemoryBus : IMediatorHandler
    {
        //构造函数注入
        private readonly IMediator mediator;
        // 事件仓储服务
        private readonly IEventStoreService eventStoreService;
        public InMemoryBus(IMediator mediator, IEventStoreService eventStoreService)
        {
            this.mediator = mediator;
            this.eventStoreService = eventStoreService;
        }

        public Task PublishEvent<TNotification>(TNotification @event, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : Event
        {
            // 除了领域通知以外的事件都保存下来
            if (!@event.MessageType.Equals("DomainNotification"))
                eventStoreService?.Save(@event);

            // MediatR中介者模式中的第二种方法，发布/订阅模式
            return mediator.Publish(@event);
        }

        public Task<TResponse> SendCommand<TResponse>(Command<TResponse> command, CancellationToken cancellationToken = default(CancellationToken))
        {
            return mediator.Send(command, cancellationToken);
        }
    }
}
