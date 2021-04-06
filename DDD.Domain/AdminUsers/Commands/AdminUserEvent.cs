﻿using DDD.Domain.Common.Interfaces;
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
    public class CreateAdminUserEvent : INotification
    {
        public AdminUser User { get; set; }
        public class AdminUserCreatedHandler : INotificationHandler<CreateAdminUserEvent>
        {
            private readonly INotificationService notification;

            public AdminUserCreatedHandler(INotificationService notification)
            {
                this.notification = notification;
            }

            public async Task Handle(CreateAdminUserEvent notification, CancellationToken cancellationToken)
            {
                //接受消息处理创建后的业务
                await this.notification.SendAsync(new MessageDto());
            }
        }
    }

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
