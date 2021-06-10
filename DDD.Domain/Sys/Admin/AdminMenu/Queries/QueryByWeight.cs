using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using DDD.Domain.Common;
using DDD.Domain.Core;
using XUCore.Ddd.Domain.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuQueryByWeight : Command<IList<AdminMenuDto>>
    {
        public bool IsMenu { get; set; }

        public class Validator : CommandValidator<AdminMenuQueryByWeight>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<AdminMenuQueryByWeight, IList<AdminMenuDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<AdminMenuDto>> Handle(AdminMenuQueryByWeight request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthMenus
                    .Where(c => c.IsMenu == request.IsMenu)
                    .OrderByDescending(c => c.Weight)
                    .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
