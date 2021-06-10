using AutoMapper;
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

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserUpdateStatusCommand : Command<int>
    {
        public long[] Ids { get; set; }
        public Status Status { get; set; }

        public class Validator : CommandValidator<AdminUserUpdateStatusCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Ids).NotEmpty().WithName("Id");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateStatusCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Show, Updated_At = DateTime.Now });
                    case Status.SoldOut:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.SoldOut, Updated_At = DateTime.Now });
                    case Status.Trash:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Trash, Deleted_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
