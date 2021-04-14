using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.AdminUsers.Dtos;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly INigelDbRepository db;
        private readonly IMapper mapper;

        public AdminUserQueryHandler(INigelDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<(SubCode, AdminUserDto)> Handle(AdminUserDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await db.Context.AdminUser.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity != null && entity.Status == false)
                return (SubCode.SoldOut, default);

            if (entity != null)
                return (SubCode.Success, mapper.Map<AdminUserDto>(entity));

            return (SubCode.Fail, default);
        }

        public async Task<(SubCode, IList<AdminUserDto>)> Handle(AdminUserListQuery request, CancellationToken cancellationToken)
        {
            // 仓储提供的单表查询

            //Expression<Func<AdminUser, bool>> selector = c => true;

            //selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            //var list = await db.GetListAsync(
            //    selector: selector,
            //    orderby: "Id desc",
            //    limit: request.Limit,
            //    cancellationToken: cancellationToken);

            //if (list != null)
            //    return (SubCode.Success, mapper.ToResult<List<AdminUser>, IList<AdminUserDto>>(list));

            //return (SubCode.Fail, default);

            // ef 直接查询

            var list = await db.Context.AdminUser
                 .WhereIf(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty())
                 .OrderBy(c => c.Id)
                 .Take(request.Limit)
                 .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            if (list != null)
                return (SubCode.Success, list);

            return (SubCode.Fail, default);
        }

        public async Task<(SubCode, PagedModel<AdminUserDto>)> Handle(AdminUserPagedListQuery request, CancellationToken cancellationToken)
        {
            // 仓储提供的单表查询

            //Expression<Func<AdminUser, bool>> selector = c => true;

            //selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

            //var page = await db.GetPagedListAsync(
            //    selector: selector,
            //    orderby: "Id desc",
            //    currentPage: request.CurrentPage,
            //    pageSize: request.PageSize,
            //    cancellationToken: cancellationToken);

            //if (page != null)
            //    return (SubCode.Success, mapper.ToPageResult<AdminUser, AdminUserDto>(page));

            //return (SubCode.Fail, default);

            // ef 直接查询

            var page = await db.Context.AdminUser
                 .WhereIf(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty())
                 .OrderBy(c => c.Id)
                 .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                 .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            if (page != null)
                return (SubCode.Success, page.Model);

            return (SubCode.Fail, default);
        }
    }
}
