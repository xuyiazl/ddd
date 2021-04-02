using FluentValidation;

namespace DDD.Domain.AdminUsers.Commands.Create
{
    public class CreateAdminUserCommandValidator : AbstractValidator<CreateAdminUserCommand>
    {
        public CreateAdminUserCommandValidator()
        {
            //RuleFor(x => x.Id).Length(5).NotEmpty();
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("账号不可为空")
                .MaximumLength(20).WithMessage(c=>$"账号不能超过20个字符，当前{c.UserName.Length}个字符");

            RuleFor(x => x.Mobile)
                .NotEmpty().WithMessage("手机号码不可为空")
                .MaximumLength(11).WithMessage(c => $"账号不能超过11个字符，当前{c.Mobile.Length}个字符");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密码不可为空")
                .MaximumLength(30).WithMessage(c => $"密码不能超过30个字符，当前{c.Password.Length}个字符");

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
