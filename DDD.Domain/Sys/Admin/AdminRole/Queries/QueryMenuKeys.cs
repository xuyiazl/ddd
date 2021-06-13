using AutoMapper;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace DDD.Domain.Sys.AdminRole
{
    /// <summary>
    /// 查询角色关联的导航id集合
    /// </summary>
    public class AdminRoleQueryMenuKeys : Command<IList<long>>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public long RoleId { get; set; }

        public class Handler : CommandHandler<AdminRoleQueryMenuKeys, IList<long>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<long>> Handle(AdminRoleQueryMenuKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.AdminAuthRoleMenus
                    .Where(c => c.RoleId == request.RoleId)
                    .OrderBy(c => c.MenuId)
                    .Select(c => c.MenuId)
                    .ToListAsync();
            }
        }
    }
}
