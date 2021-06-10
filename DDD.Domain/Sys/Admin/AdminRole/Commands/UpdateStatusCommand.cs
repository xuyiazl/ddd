using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleUpdateStatusCommand : Command<int>
    {
        public long[] Ids { get; set; }
        public Status Status { get; set; }

        public class Validator : CommandValidator<AdminRoleUpdateStatusCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Ids).NotEmpty().WithName("Id");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminRoleUpdateStatusCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<AdminRoleEntity>(c => request.Ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Show, Updated_At = DateTime.Now });
                    case Status.SoldOut:
                        return await db.UpdateAsync<AdminRoleEntity>(c => request.Ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.SoldOut, Updated_At = DateTime.Now });
                    case Status.Trash:
                        return await db.UpdateAsync<AdminRoleEntity>(c => request.Ids.Contains(c.Id), c => new AdminRoleEntity { Status = Status.Trash, Deleted_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
