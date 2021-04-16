using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Commands;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers
{
    public partial class AdminUserCommand
    {
        public class UpdateCommand : Command<(SubCode, int)>
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string Picture { get; set; }
            public string Location { get; set; }
            public string Position { get; set; }
            public string Company { get; set; }

            public override bool IsVaild()
            {
                ValidationResult = new Validator().Validate(this);
                return ValidationResult.IsValid;
            }

            public class Validator : AbstractValidator<UpdateCommand>
            {
                public Validator()
                {
                    RuleFor(x => x.Id)
                        .NotEmpty().WithMessage("Id不可为空")
                        .GreaterThan(0).WithMessage(c => $"Id必须大于0");

                    RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("名字不可为空")
                        .MaximumLength(20).WithMessage(c => $"名字不能超过20个字符，当前{c.Name.Length}个字符");

                    RuleFor(x => x.Company)
                        .MaximumLength(30).WithMessage(c => $"公司不能超过30个字符，当前{c.Company.Length}个字符");

                    RuleFor(x => x.Location)
                        .MaximumLength(30).WithMessage(c => $"位置不能超过30个字符，当前{c.Location.Length}个字符");

                    RuleFor(x => x.Picture)
                        .MaximumLength(250).WithMessage(c => $"头像不能超过250个字符，当前{c.Picture.Length}个字符");

                    RuleFor(x => x.Position)
                        .MaximumLength(20).WithMessage(c => $"职位不能超过20个字符，当前{c.Position.Length}个字符");
                }
            }

            internal class AdminUserCommandHandler : CommandHandler,
                IRequestHandler<UpdateCommand, (SubCode, int)>
            {
                private readonly INigelDbRepository db;

                public AdminUserCommandHandler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
                {
                    this.db = db;
                }

                public async Task<(SubCode, int)> Handle(UpdateCommand request, CancellationToken cancellationToken)
                {
                    var entity = await db.Context.AdminUser.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

                    if (entity == null)
                        return (SubCode.Undefind, 0);

                    entity.Id = request.Id;
                    entity.Name = request.Name;
                    entity.Company = request.Company;
                    entity.Location = request.Location;
                    entity.Picture = request.Picture;
                    entity.Position = request.Position;

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
}
