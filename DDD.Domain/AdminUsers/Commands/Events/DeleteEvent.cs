using DDD.Domain.Core.Events;
using DDD.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers
{
    internal class DeleteEvent : Event
    {
        public long Id { get; set; }
        public DeleteEvent(long id)
        {
            Id = id;
            AggregateId = id;
            AggregateType = nameof(AdminUserEntity);
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        internal class Handler :
            INotificationHandler<DeleteEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理删除后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(DeleteEvent notification, CancellationToken cancellationToken)
            {

            }
        }
    }
}
