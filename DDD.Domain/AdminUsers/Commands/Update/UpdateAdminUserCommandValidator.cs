using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers.Commands.Update
{
    public class UpdateAdminUserCommandValidator : AbstractValidator<UpdateAdminUserCommand>
    {
        public UpdateAdminUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).NotEmpty();
            RuleFor(x => x.Name).MaximumLength(20).NotEmpty();
            RuleFor(x => x.Company).MaximumLength(30);
            RuleFor(x => x.Location).MaximumLength(30);
            RuleFor(x => x.Picture).MaximumLength(250);
            RuleFor(x => x.Position).MaximumLength(20);
        }
    }
}
