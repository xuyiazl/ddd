using DDD.Domain.Common;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore;

namespace DDD.Domain.AdminUsers.Queries.GetList
{
    public class AdminUserListQuery : IRequest<Result<List<AdminUser>>>
    {
        public int Limit { get; set; }
        public string Keyword { get; set; }

        public class AdminUserListQueryHandler : IRequestHandler<AdminUserListQuery, Result<List<AdminUser>>>
        {
            private readonly INigelDbRepository<AdminUser> repository;

            public AdminUserListQueryHandler(INigelDbRepository<AdminUser> repository)
            {
                this.repository = repository;
            }

            public async Task<Result<List<AdminUser>>> Handle(AdminUserListQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<AdminUser, bool>> selector = c => true;

                selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                var list = await repository.GetListAsync(
                    selector: selector,
                    orderby: "Id desc",
                    limit: request.Limit,
                    cancellationToken: cancellationToken);

                return Return.Success(SubCode.Success, list);
            }
        }
    }
}
