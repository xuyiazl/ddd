using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserDeleteCommand : Command<int>
    {
        public long[] Ids { get; set; }

        public class Validator : CommandValidator<AdminUserDeleteCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Ids).NotEmpty().WithName("Id");
            }
        }

        public class Handler : CommandHandler<AdminUserDeleteCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    //删除登录记录
                    await db.DeleteAsync<LoginRecordEntity>(c => request.Ids.Contains(c.AdminId));
                    //删除关联的角色
                    await db.DeleteAsync<AdminUserRoleEntity>(c => request.Ids.Contains(c.UserId));
                }

                return res;
            }
        }
    }
}
