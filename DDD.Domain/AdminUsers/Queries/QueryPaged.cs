using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Core;
using DDD.Domain.Core.Commands;
using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;


namespace DDD.Domain.AdminUsers
{
    public partial class AdminUserCommand
    {
        public class QueryPaged : Command<(SubCode, PagedModel<AdminUserDto>)>
        {
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public string Keyword { get; set; }

            public override bool IsVaild()
            {
                ValidationResult = new Validator().Validate(this);
                return ValidationResult.IsValid;
            }

            public class Validator : AbstractValidator<QueryPaged>
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

            internal class Handler : IRequestHandler<QueryPaged, (SubCode, PagedModel<AdminUserDto>)>
            {
                private readonly INigelDbRepository db;
                private readonly IMapper mapper;

                public Handler(INigelDbRepository db, IMapper mapper)
                {
                    this.db = db;
                    this.mapper = mapper;
                }

                public async Task<(SubCode, PagedModel<AdminUserDto>)> Handle(QueryPaged request, CancellationToken cancellationToken)
                {
                    // 仓储提供的单表查询

                    //Expression<Func<AdminUser, bool>> selector = c => true;

                    //selector = selector.And(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty());

                    //var page = await db.GetPagedListAsync(
                    //    selector: selector,
                    //    orderby: "Id desc",
                    //    currentPage: request.CurrentPage,
                    //    pageSize: request.PageSize,
                    //    cancellationToken: cancellationToken);

                    //if (page != null)
                    //    return (SubCode.Success, mapper.ToPageResult<AdminUser, AdminUserDto>(page));

                    //return (SubCode.Fail, default);

                    // ef 直接查询

                    var page = await db.Context.AdminUser
                         .WhereIf(c => c.Name.Contains(request.Keyword), !request.Keyword.IsEmpty())
                         .OrderBy(c => c.Id)
                         .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                         .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                    if (page != null)
                        return (SubCode.Success, page.Model);

                    return (SubCode.Fail, default);
                }
            }
        }
    }
}
