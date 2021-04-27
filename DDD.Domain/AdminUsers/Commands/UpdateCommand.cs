using DDD.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserUpdateCommand : Command<(SubCode, int)>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminUserUpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id不可为空")
                    .GreaterThan(0).WithMessage(c => $"Id必须大于0");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("名字不可为空")
                    .MaximumLength(20).WithMessage(c => $"名字不能超过20个字符，当前{c.Name.Length}个字符");

                RuleFor(x => x.Picture)
                    .MaximumLength(250).WithMessage(c => $"头像不能超过250个字符，当前{c.Picture.Length}个字符");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateCommand, (SubCode, int)>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            public override async Task<(SubCode, int)> Handle(AdminUserUpdateCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Context.AdminUser.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

                if (entity == null)
                    return (SubCode.Undefind, 0);

                entity.Id = request.Id;
                entity.Name = request.Name;
                entity.Picture = request.Picture;

                entity.Updated_At = DateTime.Now;

                var res = db.Update(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new UpdateEvent(entity.Id, entity), cancellationToken);

                    return (SubCode.Success, res);
                }
                return (SubCode.Fail, res);
            }
        }
    }

}
