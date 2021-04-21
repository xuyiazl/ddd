using DDD.Domain.Core.Entities;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Events;

namespace DDD.Domain.AdminUsers
{
    public class DeleteEvent : Event
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
        public class Handler : NotificationEventHandler<DeleteEvent>
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
            public override async Task Handle(DeleteEvent notification, CancellationToken cancellationToken)
            {

            }
        }
    }
}
