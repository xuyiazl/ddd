using AutoMapper;
using FluentValidation;
using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserRelevanceRoleCommand : Command<int>
    {
        public long AdminId { get; set; }
        public long[] RoleIds { get; set; }


        public class Validator : CommandValidator<AdminUserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }

        public class Handler : CommandHandler<AdminUserRelevanceRoleCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken)
            {
                //先清空用户的角色，确保没有冗余的数据
                await db.DeleteAsync<AdminUserRoleEntity>(c => c.UserId == request.AdminId);

                var userRoles = Array.ConvertAll(request.RoleIds, roleid => new AdminUserRoleEntity
                {
                    RoleId = roleid,
                    UserId = request.AdminId
                });

                //添加角色
                if (userRoles.Length > 0)
                    return await db.AddAsync(userRoles);

                return 1;
            }
        }
    }
}
