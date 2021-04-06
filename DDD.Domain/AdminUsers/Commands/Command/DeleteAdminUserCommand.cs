using DDD.Domain.Common;
using FluentValidation;
using MediatR;
using XUCore.NetCore;

namespace DDD.Domain.AdminUsers.Commands
{
    public class DeleteAdminUserCommand : AbstractValidator<DeleteAdminUserCommand>, IRequest<(SubCode, int)>
    {
        public long Id { get; set; }

        public DeleteAdminUserCommand()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id不可为空")
                .GreaterThan(0).WithMessage(c => $"Id必须大于0");
        }
    }
}
