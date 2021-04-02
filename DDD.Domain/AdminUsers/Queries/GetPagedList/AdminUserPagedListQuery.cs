using DDD.Domain.AdminUsers.Queries.GetList;
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
using XUCore.Paging;

namespace DDD.Domain.AdminUsers.Queries.GetPagedList
{
    public class AdminUserPagedListQuery : IRequest<Result<PagedList<AdminUser>>>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }

        public class AdminUserListQueryHandler : IRequestHandler<AdminUserPagedListQuery, Result<PagedList<AdminUser>>>
        {
            private readonly INigelDbRepository<AdminUser> repository;

            public AdminUserListQueryHandler(INigelDbRepository<AdminUser> repository)
            {
                this.repository = repository;
            }

            public async Task<Result<PagedList<AdminUser>>> Handle(AdminUserPagedListQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<AdminUser, bool>> selector = c => true;

                selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                var page = await repository.GetPagedListAsync(
                    selector: selector,
                    orderby: "Id desc",
                    currentPage: request.CurrentPage,
                    pageSize: request.PageSize,
                    cancellationToken: cancellationToken);

                if (page == null)
                    return Return.Fail(SubCode.Fail, default(PagedList<AdminUser>));

                return Return.Success(SubCode.Success, page);
            }
        }
    }
}
