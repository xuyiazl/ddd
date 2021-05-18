using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserDeleteCommand : CommandId<int>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandIdValidator<AdminUserDeleteCommand, int>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<AdminUserDeleteCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [RedisCacheRemove(HashKey = RedisKey.Admin, Key = "{Id}")]
            public override async Task<int> Handle(AdminUserDeleteCommand request, CancellationToken cancellationToken)
            {
                var has = await db.Context.AdminUser.AnyAsync(c => c.Id == request.Id, cancellationToken);

                if (!has)
                    return 0;

                var res = await db.DeleteAsync<AdminUserEntity>(c => c.Id == request.Id);

                if (res > 0)

                    await bus.PublishEvent(new DeleteEvent(request.Id), cancellationToken);

                return res;
            }
        }
    }

}
