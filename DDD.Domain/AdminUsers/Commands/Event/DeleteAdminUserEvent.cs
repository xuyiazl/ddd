using DDD.Domain.Common.Interfaces;
using DDD.Domain.Notifications.Models;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers.Commands.Event
{
    public class DeleteAdminUserEvent : INotification
    {
        public long Id { get; set; }
        public class AdminUserCreatedHandler : INotificationHandler<DeleteAdminUserEvent>
        {
            public AdminUserCreatedHandler()
            {
            }

            public async Task Handle(DeleteAdminUserEvent notification, CancellationToken cancellationToken)
            {
                //接受消息处理删除后的业务
            }
        }
    }
}
