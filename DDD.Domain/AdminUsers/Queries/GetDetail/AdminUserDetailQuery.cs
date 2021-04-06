using DDD.Domain.Common;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.AdminUsers.Queries.GetDetail
{
    public class AdminUserDetailQuery : IRequest<Result<AdminUser>>
    {
        public long Id { get; set; }

        public class AdminUserListQueryHandler : IRequestHandler<AdminUserDetailQuery, Result<AdminUser>>
        {
            private readonly INigelDbUnitOfWork unitOfWork;

            public AdminUserListQueryHandler(INigelDbUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }

            public async Task<Result<AdminUser>> Handle(AdminUserDetailQuery request, CancellationToken cancellationToken)
            {
                var entity = await unitOfWork.GetByIdAsync<AdminUser>(request.Id, cancellationToken: cancellationToken);

                return Return.Success(SubCode.Success, entity);
            }
        }
    }
}
