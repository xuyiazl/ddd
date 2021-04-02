using FluentValidation;

namespace DDD.Domain.AdminUsers.Commands.Create
{
    public class CreateAdminUserCommandValidator : AbstractValidator<CreateAdminUserCommand>
    {
        public CreateAdminUserCommandValidator()
        {
            //RuleFor(x => x.Id).Length(5).NotEmpty();
            RuleFor(x => x.UserName).MaximumLength(20).NotEmpty();
            RuleFor(x => x.Mobile).MaximumLength(11).NotEmpty();
            RuleFor(x => x.Password).MaximumLength(30).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(20).NotEmpty();
            RuleFor(x => x.Company).MaximumLength(30);
            RuleFor(x => x.Location).MaximumLength(30);
            RuleFor(x => x.Picture).MaximumLength(250);
            RuleFor(x => x.Position).MaximumLength(20);
        }
    }
}
