using AutoMapper;
using DDD.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserAnyByAccount : Command<bool>
    {
        public AccountMode AccountMode { get; set; }
        public string Account { get; set; }
        public long NotId { get; set; }

        public class Validator : CommandValidator<AdminUserAnyByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler : IRequestHandler<AdminUserAnyByAccount, bool>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public async Task<bool> Handle(AdminUserAnyByAccount request, CancellationToken cancellationToken)
            {
                if (request.NotId > 0)
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.Context.AdminUser.AnyAsync(c => c.Id != request.NotId && c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.Context.AdminUser.AnyAsync(c => c.Id != request.NotId && c.Mobile == request.Account, cancellationToken);
                    }
                }
                else
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.Context.AdminUser.AnyAsync(c => c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.Context.AdminUser.AnyAsync(c => c.Mobile == request.Account, cancellationToken);
                    }
                }

                return false;
            }
        }
    }
}
