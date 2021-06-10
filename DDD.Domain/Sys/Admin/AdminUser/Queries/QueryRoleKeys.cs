using AutoMapper;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserQueryRoleKeys : Command<IList<long>>
    {
        public long AdminId { get; set; }

        public class Validator : CommandValidator<AdminUserQueryRoleKeys>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }

        public class Handler :
            IRequestHandler<AdminUserQueryRoleKeys, IList<long>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<IList<long>> Handle(AdminUserQueryRoleKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.AdminAuthUserRole
                    .Where(c => c.UserId == request.AdminId)
                    .Select(c => c.RoleId)
                    .ToListAsync();
            }
        }
    }
}
