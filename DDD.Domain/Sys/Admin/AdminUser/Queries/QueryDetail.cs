using AutoMapper;
using AutoMapper.QueryableExtensions;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserQueryDetail : Command<AdminUserDto>
    {
        [Required]
        public long Id { get; set; }

        public class Validator : CommandValidator<AdminUserQueryDetail>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
            }
        }

        public class Handler : CommandHandler<AdminUserQueryDetail, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminUserDto> Handle(AdminUserQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminUser
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}
