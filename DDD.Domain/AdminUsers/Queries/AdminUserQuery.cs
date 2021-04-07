using DDD.Domain.AdminUsers.Dtos;
using DDD.Domain.Core;
using DDD.Domain.Core.Commands;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Domain.AdminUsers.Queries
{
    public class AdminUserDetailQuery : Command<(SubCode, AdminUserDto)>
    {
        public long Id { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : AbstractValidator<AdminUserDetailQuery>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id不可为空")
                    .GreaterThan(0).WithMessage(c => $"Id必须大于0");
            }
        }
    }

    public class AdminUserListQuery : Command<(SubCode, IList<AdminUserDto>)>
    {
        public int Limit { get; set; }
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : AbstractValidator<AdminUserListQuery>
        {
            public Validator()
            {
                RuleFor(x => x.Limit)
                    .NotEmpty().WithMessage("limit不可为空")
                    .GreaterThan(0).WithMessage(c => $"limit必须大于0")
                    .LessThanOrEqualTo(100).WithMessage(c => $"limit必须小于等于100");
            }
        }
    }

    public class AdminUserPagedListQuery : Command<(SubCode, PagedModel<AdminUserDto>)>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : AbstractValidator<AdminUserPagedListQuery>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage)
                    .NotEmpty().WithMessage("页码不可为空")
                    .GreaterThan(0).WithMessage(c => $"页码必须大于0");

                RuleFor(x => x.PageSize)
                    .NotEmpty().WithMessage("分页大小不可为空")
                    .GreaterThan(0).WithMessage(c => $"分页大小必须大于0")
                    .LessThanOrEqualTo(100).WithMessage(c => $"分页大小必须小于等于100");
            }
        }
    }
}
