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

namespace DDD.Domain.AdminUsers.Commands.Update
{
    public class UpdateAdminUserEvent : INotification
    {
        public AdminUser User { get; set; }
        public class AdminUserCreatedHandler : INotificationHandler<UpdateAdminUserEvent>
        {
            private readonly INotificationService notification;

            public AdminUserCreatedHandler(INotificationService notification)
            {
                this.notification = notification;
            }

            public async Task Handle(UpdateAdminUserEvent notification, CancellationToken cancellationToken)
            {
                //接受消息处理修改后的业务
            }
        }
    }
}
