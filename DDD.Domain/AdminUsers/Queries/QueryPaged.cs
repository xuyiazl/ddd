using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;


namespace DDD.Domain.AdminUsers
{
    public class AdminUserQueryPaged : CommandPage<PagedModel<AdminUserDto>>
    {
        public string Keyword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandPageValidator<AdminUserQueryPaged, PagedModel<AdminUserDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }

        public class Handler : CommandHandler<AdminUserQueryPaged, PagedModel<AdminUserDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<AdminUserDto>> Handle(AdminUserQueryPaged request, CancellationToken cancellationToken)
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

                return page?.ToModel();
            }
        }
    }
}
