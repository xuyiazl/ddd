using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers.Commands
{
    /// <summary>
    /// 事件通知操作
    /// </summary>
    public class AdminUserEventHandler :
        INotificationHandler<CreateAdminUserEvent>,
        INotificationHandler<UpdateAdminUserEvent>,
        INotificationHandler<DeleteAdminUserEvent>
    {
        public AdminUserEventHandler()
        {
        }
        /// <summary>
        /// 接受消息处理创建后的业务
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(CreateAdminUserEvent notification, CancellationToken cancellationToken)
        {
            
        }
        /// <summary>
        /// 接受消息处理修改后的业务
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(UpdateAdminUserEvent notification, CancellationToken cancellationToken)
        {

        }
        /// <summary>
        /// 接受消息处理删除后的业务
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task Handle(DeleteAdminUserEvent notification, CancellationToken cancellationToken)
        {

        }
    }
}
