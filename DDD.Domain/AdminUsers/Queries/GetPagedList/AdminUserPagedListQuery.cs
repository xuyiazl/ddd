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
using XUCore.NetCore.Data.DbService;
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
            private readonly INigelDbUnitOfWork unitOfWork;

            public AdminUserListQueryHandler(INigelDbUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
            }

            public async Task<Result<PagedList<AdminUser>>> Handle(AdminUserPagedListQuery request, CancellationToken cancellationToken)
            {
                Expression<Func<AdminUser, bool>> selector = c => true;

                selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                var page = await unitOfWork.GetPagedListAsync<AdminUser>(
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
