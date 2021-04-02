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

namespace DDD.Domain.AdminUsers.Queries.GetDetail
{
    public class AdminUserDetailQuery : IRequest<Result<AdminUser>>
    {
        public int Id { get; set; }

        public class AdminUserListQueryHandler : IRequestHandler<AdminUserDetailQuery, Result<AdminUser>>
        {
            private readonly INigelDbRepository<AdminUser> repository;

            public AdminUserListQueryHandler(INigelDbRepository<AdminUser> repository)
            {
                this.repository = repository;
            }

            public async Task<Result<AdminUser>> Handle(AdminUserDetailQuery request, CancellationToken cancellationToken)
            {
                var entity = await repository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

                return ResultModel.Success(SubCode.Success, entity);
            }
        }
    }
}
