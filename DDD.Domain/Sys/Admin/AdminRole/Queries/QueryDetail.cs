using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleQueryDetail : Command<AdminRoleDto>
    {
        public long Id { get; set; }

        public class Validator : CommandValidator<AdminRoleQueryDetail>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
            }
        }

        public class Handler :
            IRequestHandler<AdminRoleQueryDetail, AdminRoleDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<AdminRoleDto> Handle(AdminRoleQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthRole
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}