using MediatR;
using XUCore.Ddd.Domain.Events;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Sys.AdminMenu
{
    public class CreateEvent : Event
    {
        public long Id { get; set; }
        public AdminMenuEntity Menu { get; set; }
        public CreateEvent(long id, AdminMenuEntity menu)
        {
            Id = id;
            Menu = menu;
            AggregateId = id.ToString();
        }

        /// <summary>
        /// 事件通知操作
        /// </summary>
        public class Handler : NotificationEventHandler<CreateEvent>
        {
            public Handler()
            {
            }
            /// <summary>
            /// 接受消息处理创建后的业务
            /// </summary>
            /// <param name="notification"></param>
            /// <param name="cancellationToken"></param>
            /// <returns></returns>
            public override async Task Handle(CreateEvent notification, CancellationToken cancellationToken)
            {

                await Task.CompletedTask;
            }
        }
    }
}
