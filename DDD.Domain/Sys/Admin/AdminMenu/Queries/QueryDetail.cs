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

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuQueryDetail : Command<AdminMenuDto>
    {
        public long Id { get; set; }

        public class Validator : CommandValidator<AdminMenuQueryDetail>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
            }
        }

        public class Handler :
            IRequestHandler<AdminMenuQueryDetail, AdminMenuDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<AdminMenuDto> Handle(AdminMenuQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthMenus
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}
