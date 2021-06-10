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

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserQueryByAccount : Command<AdminUserDto>
    {
        public AccountMode AccountMode { get; set; }
        public string Account { get; set; }

        public class Validator : CommandValidator<AdminUserQueryByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler :
            IRequestHandler<AdminUserQueryByAccount, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<AdminUserDto> Handle(AdminUserQueryByAccount request, CancellationToken cancellationToken)
            {
                switch (request.AccountMode)
                {
                    case AccountMode.UserName:

                        return await db.Context.AdminUser
                            .Where(c => c.UserName.Equals(request.Account))
                            .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);

                    case AccountMode.Mobile:

                        return await db.Context.AdminUser
                            .Where(c => c.Mobile.Equals(request.Account))
                            .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);
                }

                return null;
            }
        }
    }
}
