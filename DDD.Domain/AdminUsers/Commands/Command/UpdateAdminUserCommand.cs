using DDD.Domain.Common;
using FluentValidation;
using MediatR;
using XUCore.NetCore;

namespace DDD.Domain.AdminUsers.Commands
{
    public class UpdateAdminUserCommand : AbstractValidator<UpdateAdminUserCommand>,IRequest<(SubCode, int)>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }

        public UpdateAdminUserCommand()
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
}
