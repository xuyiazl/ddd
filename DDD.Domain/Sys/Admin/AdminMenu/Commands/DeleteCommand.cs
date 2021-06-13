using FluentValidation;
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

namespace DDD.Domain.Sys.AdminMenu
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class AdminMenuDeleteCommand : CommandIds<int, long>
    {
        public class Validator : CommandIdsValidator<AdminMenuDeleteCommand, int, long>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<AdminMenuDeleteCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminMenuDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<AdminMenuEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    await db.DeleteAsync<AdminRoleMenuEntity>(c => request.Ids.Contains(c.MenuId));
                }

                return res;
            }
        }
    }
}
