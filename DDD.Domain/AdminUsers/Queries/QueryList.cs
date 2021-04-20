using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Core.Commands;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;


namespace DDD.Domain.AdminUsers
{
    public partial class AdminUserCommand
    {
        public class QueryList : Command<(SubCode, IList<AdminUserDto>)>
        {
            public int Limit { get; set; }
            public string Keyword { get; set; }

            public override bool IsVaild()
            {
                ValidationResult = new Validator().Validate(this);
                return ValidationResult.IsValid;
            }
            public class Validator : AbstractValidator<QueryList>
            {
                public Validator()
                {
                    RuleFor(x => x.Limit)
                        .NotEmpty().WithMessage("limit不可为空")
                        .GreaterThan(0).WithMessage(c => $"limit必须大于0")
                        .LessThanOrEqualTo(100).WithMessage(c => $"limit必须小于等于100");
                }
            }
            public class Handler : CommandHandler<QueryList, (SubCode, IList<AdminUserDto>)>
            {
                private readonly INigelDbRepository db;
                private readonly IMapper mapper;

                public Handler(INigelDbRepository db, IMapper mapper)
                {
                    this.db = db;
                    this.mapper = mapper;
                }

                public override async Task<(SubCode, IList<AdminUserDto>)> Handle(QueryList request, CancellationToken cancellationToken)
                {
                    // 仓储提供的单表查询

                    //Expression<Func<AdminUser, bool>> selector = c => true;

                    //selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                    //var list = await db.GetListAsync(
                    //    selector: selector,
                    //    orderby: "Id desc",
                    //    limit: request.Limit,
                    //    cancellationToken: cancellationToken);

                    //if (list != null)
                    //    return (SubCode.Success, mapper.ToResult<List<AdminUser>, IList<AdminUserDto>>(list));

                    //return (SubCode.Fail, default);

                    // ef 直接查询

                    var list = await db.Context.AdminUser
                         .WhereIf(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty())
                         .OrderBy(c => c.Id)
                         .Take(request.Limit)
                         .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                         .ToListAsync(cancellationToken);

                    if (list != null)
                        return (SubCode.Success, list);

                    return (SubCode.Fail, default);
                }
            }
        }
    }
}
