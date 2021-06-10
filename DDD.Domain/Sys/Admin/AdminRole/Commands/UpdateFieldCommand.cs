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

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleUpdateFieldCommand : Command<int>
    {
        public long Id { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }

        public class Validator : CommandValidator<AdminRoleUpdateFieldCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<AdminRoleUpdateFieldCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<AdminRoleEntity>(c => c.Id == request.Id, c => new AdminRoleEntity() { Name = request.Value, Updated_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
