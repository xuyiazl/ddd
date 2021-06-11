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
using XUCore.Paging;

namespace DDD.Domain.Sys.LoginRecord
{
    public class LoginRecordQueryList : Command<IList<LoginRecordDto>>
    {
        public int Limit { get; set; }
        public long AdminId { get; set; }

        public class Validator : CommandValidator<LoginRecordQueryList>
        {
            public Validator()
            {
                RuleFor(x => x.Limit)
                    .NotEmpty().WithMessage("limit不可为空")
                    .GreaterThan(0).WithMessage(c => $"limit必须大于0")
                    .LessThanOrEqualTo(100).WithMessage(c => $"limit必须小于等于100");

                RuleFor(x => x.AdminId)
                    .NotEmpty().WithMessage("AdminId不可为空")
                    .GreaterThan(0).WithMessage(c => $"AdminId必须大于0");
            }
        }

        public class Handler : CommandHandler<LoginRecordQueryList, IList<LoginRecordDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<LoginRecordDto>> Handle(LoginRecordQueryList request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .Where(c => c.AdminId == request.AdminId)

                    .OrderByDescending(c => c.LoginTime)
                    .Take(request.Limit)

                    .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
