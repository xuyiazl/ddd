using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.AdminUsers.Commands.Delete
{
    public class DeleteAdminUserCommandValidator : AbstractValidator<DeleteAdminUserCommand>
    {
        public DeleteAdminUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id不可为空")
                .GreaterThan(0).WithMessage(c => $"Id必须大于0");
        }
    }
}
