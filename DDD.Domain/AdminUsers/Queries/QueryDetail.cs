﻿using AutoMapper;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Core.Commands;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;


namespace DDD.Domain.AdminUsers
{
    public partial class AdminUserCommand
    {
        public class QueryDetail : Command<(SubCode, AdminUserDto)>
        {
            public long Id { get; set; }

            public override bool IsVaild()
            {
                ValidationResult = new Validator().Validate(this);
                return ValidationResult.IsValid;
            }
            public class Validator : AbstractValidator<QueryDetail>
            {
                public Validator()
                {
                    RuleFor(x => x.Id)
                        .NotEmpty().WithMessage("Id不可为空")
                        .GreaterThan(0).WithMessage(c => $"Id必须大于0");
                }
            }

            public class Handler :
                IRequestHandler<QueryDetail, (SubCode, AdminUserDto)>
            {
                private readonly INigelDbRepository db;
                private readonly IMapper mapper;

                public Handler(INigelDbRepository db, IMapper mapper)
                {
                    this.db = db;
                    this.mapper = mapper;
                }

                public async Task<(SubCode, AdminUserDto)> Handle(QueryDetail request, CancellationToken cancellationToken)
                {
                    var entity = await db.Context.AdminUser.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                    if (entity != null && entity.Status == false)
                        return (SubCode.SoldOut, default);

                    if (entity != null)
                        return (SubCode.Success, mapper.Map<AdminUserDto>(entity));

                    return (SubCode.Fail, default);
                }
            }
        }
    }
}
