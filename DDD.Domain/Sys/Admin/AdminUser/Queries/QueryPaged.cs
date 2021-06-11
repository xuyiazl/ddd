using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserQueryPaged : Command<PagedModel<AdminUserDto>>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Field { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public Status Status { get; set; }


        public class Validator : CommandValidator<AdminUserQueryPaged>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage).NotEmpty().GreaterThan(0).WithName("页码");
                RuleFor(x => x.PageSize).NotEmpty().GreaterThan(0).LessThanOrEqualTo(150).WithName("分页大小");
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
                var res = await db.Context.AdminUser

                    .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                    .WhereIf(c =>
                                c.Name.Contains(request.Search) ||
                                c.Mobile.Contains(request.Search) ||
                                c.UserName.Contains(request.Search), !request.Search.IsEmpty())

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.Model;
            }
        }
    }
}
