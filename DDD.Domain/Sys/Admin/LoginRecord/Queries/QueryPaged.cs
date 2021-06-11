using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DDD.Domain.Core;
using XUCore.Ddd.Domain.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;

namespace DDD.Domain.Sys.LoginRecord
{
    public class LoginRecordQueryPaged : Command<PagedModel<LoginRecordDto>>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Field { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }


        public class Validator : CommandValidator<LoginRecordQueryPaged>
        {
            public Validator()
            {
                RuleFor(x => x.CurrentPage)
                    .NotEmpty().WithMessage("页码不可为空")
                    .GreaterThan(0).WithMessage(c => $"页码必须大于0");

                RuleFor(x => x.PageSize)
                    .NotEmpty().WithMessage("分页大小不可为空")
                    .GreaterThan(0).WithMessage(c => $"分页大小必须大于0")
                    .LessThanOrEqualTo(150).WithMessage(c => $"分页大小必须小于等于150");
            }
        }
        public class Handler : CommandHandler<LoginRecordQueryPaged, PagedModel<LoginRecordDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<LoginRecordDto>> Handle(LoginRecordQueryPaged request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .WhereIf(c => c.Name.Contains(request.Search) || c.Mobile.Contains(request.Search) || c.UserName.Contains(request.Search), !string.IsNullOrEmpty(request.Search))

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.Model;
            }
        }
    }
}
