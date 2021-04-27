using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserCreateCommand : Command<(SubCode, int)>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminUserCreateCommand>
        {
            public Validator()
            {
                //RuleFor(x => x.Id).Length(5).NotEmpty();
                RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("账号不可为空")
                    .MaximumLength(20).WithMessage(c => $"账号不能超过20个字符，当前{c.UserName.Length}个字符");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("密码不可为空")
                    .MaximumLength(30).WithMessage(c => $"密码不能超过30个字符，当前{c.Password.Length}个字符");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("名字不可为空")
                    .MaximumLength(20).WithMessage(c => $"名字不能超过20个字符，当前{c.Name.Length}个字符");

                RuleFor(x => x.Picture)
                    .MaximumLength(250).WithMessage(c => $"头像不能超过250个字符，当前{c.Picture.Length}个字符");
            }
        }

        public class Handler : CommandHandler<AdminUserCreateCommand, (SubCode, int)>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            public override async Task<(SubCode, int)> Handle(AdminUserCreateCommand request, CancellationToken cancellationToken)
            {
                //await bus.PublishEvent(new DomainNotification("", "开始注册...."), cancellationToken);

                var entity = new AdminUserEntity
                {
                    Name = request.Name,
                    Password = request.Password,
                    Picture = request.Picture,
                    Status = Status.Show,
                    UserName = request.UserName,

                    Updated_At = null,
                    Deleted_At = null,
                    Created_At = DateTime.Now
                };

                var res = db.Add(entity);

                //await bus.PublishEvent(new DomainNotification("", "结束注册...."), cancellationToken);

                if (res > 0)
                {
                    await bus.PublishEvent(new CreateEvent(entity.Id, entity), cancellationToken);

                    return (SubCode.Success, res);
                }
                else
                    return (SubCode.Fail, res);
            }
        }
    }
}
