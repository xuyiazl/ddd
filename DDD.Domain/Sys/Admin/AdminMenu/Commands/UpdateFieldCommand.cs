using FluentValidation;
using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuUpdateFieldCommand : Command<int>
    {
        public long Id { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }


        public class Validator : CommandValidator<AdminMenuUpdateFieldCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<AdminMenuUpdateFieldCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminMenuUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "icon":
                        return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == request.Id, c => new AdminMenuEntity() { Icon = request.Value, Updated_At = DateTime.Now });
                    case "url":
                        return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == request.Id, c => new AdminMenuEntity() { Url = request.Value, Updated_At = DateTime.Now });
                    case "onlycode":
                        return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == request.Id, c => new AdminMenuEntity() { OnlyCode = request.Value, Updated_At = DateTime.Now });
                    case "weight":
                        return await db.UpdateAsync<AdminMenuEntity>(c => c.Id == request.Id, c => new AdminMenuEntity() { Weight = request.Value.ToInt(), Updated_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
