using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleQueryAll : Command<IList<AdminRoleDto>>
    {
        public class Validator : CommandValidator<AdminRoleQueryAll>
        {
            public Validator()
            {

            }
        }

        public class Handler :
            IRequestHandler<AdminRoleQueryAll, IList<AdminRoleDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<IList<AdminRoleDto>> Handle(AdminRoleQueryAll request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthRole
                    .Where(c => c.Status == Status.Show)
                    .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
