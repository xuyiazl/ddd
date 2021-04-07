using AutoMapper;
using DDD.Domain.AdminUsers.Dtos;
using DDD.Domain.Core;
using DDD.Domain.Core.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore.Data.DbService;
using XUCore.Paging;

namespace DDD.Domain.AdminUsers.Queries
{
    public class AdminUserQueryHandler :
        IRequestHandler<AdminUserDetailQuery, (SubCode, AdminUserDto)>,
        IRequestHandler<AdminUserListQuery, (SubCode, IList<AdminUserDto>)>,
        IRequestHandler<AdminUserPagedListQuery, (SubCode, PagedModel<AdminUserDto>)>
    {
        private readonly INigelDbUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminUserQueryHandler(INigelDbUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<(SubCode, AdminUserDto)> Handle(AdminUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.GetByIdAsync<AdminUser>(request.Id, cancellationToken: cancellationToken);

            if (entity != null && entity.Status == false)
                return (SubCode.SoldOut, default);

            if (entity != null)
                return (SubCode.Success, mapper.ToResult<AdminUser, AdminUserDto>(entity));

            return (SubCode.Fail, default);
        }

        public async Task<(SubCode, IList<AdminUserDto>)> Handle(AdminUserListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AdminUser, bool>> selector = c => true;

            selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var list = await unitOfWork.GetListAsync<AdminUser>(
                selector: selector,
                orderby: "Id desc",
                limit: request.Limit,
                cancellationToken: cancellationToken);

            if (list != null)
                return (SubCode.Success, mapper.ToResult<List<AdminUser>, IList<AdminUserDto>>(list));

            return (SubCode.Fail, default);
        }

        public async Task<(SubCode, PagedModel<AdminUserDto>)> Handle(AdminUserPagedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AdminUser, bool>> selector = c => true;

            selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            var page = await unitOfWork.GetPagedListAsync<AdminUser>(
                selector: selector,
                orderby: "Id desc",
                currentPage: request.CurrentPage,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            if (page != null)
                return (SubCode.Success, mapper.ToPageResult<AdminUser, AdminUserDto>(page));

            return (SubCode.Fail, default);
        }
    }
}
