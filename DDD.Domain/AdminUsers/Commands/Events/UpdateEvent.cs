using DDD.Domain.Core.Events;
using DDD.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers
{
    public class UpdateEvent : Event
    {
        public long Id { get; set; }
        public AdminUserEntity User { get; set; }
        public UpdateEvent(long id, AdminUserEntity user)
        {
            Id = id;
            User = user;
            AggregateId = id;
            AggregateType = nameof(AdminUserEntity);
        }
        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : INotificationHandler<UpdateEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理修改后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public async Task Handle(UpdateEvent notification, CancellationToken cancellationToken)
            {

            }
        }
    }
}
