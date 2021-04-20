﻿using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.AdminUsers
{
    public partial class AdminUserCommand
    {
        public class DeleteCommand : Command<(SubCode, int)>
        {
            public long Id { get; set; }

            public override bool IsVaild()
            {
                ValidationResult = new Validator().Validate(this);
                return ValidationResult.IsValid;
            }

            public class Validator : CommandValidator<DeleteCommand>
            {
                public Validator()
                {
                    RuleFor(x => x.Id)
                        .NotEmpty().WithMessage("Id不可为空")
                        .GreaterThan(0).WithMessage(c => $"Id必须大于0");
                }
            }

            public class Handler : CommandHandler<DeleteCommand, (SubCode, int)>
            {
                private readonly INigelDbRepository db;

                public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
                {
                    this.db = db;
                }

                public override async Task<(SubCode, int)> Handle(DeleteCommand request, CancellationToken cancellationToken)
                {
                    var has = await db.Context.AdminUser.AnyAsync(c => c.Id == request.Id, cancellationToken);

                    if (!has)
                        return (SubCode.Undefind, 0);

                    var res = await db.DeleteAsync<AdminUserEntity>(c => c.Id == request.Id);

                    if (res > 0)
                    {
                        await bus.PublishEvent(new DeleteEvent(request.Id), cancellationToken);

                        return (SubCode.Success, res);
                    }
                    return (SubCode.Fail, res);
                }
            }
        }
    }
}