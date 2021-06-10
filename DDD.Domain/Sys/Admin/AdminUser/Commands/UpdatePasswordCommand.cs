using AutoMapper;
using FluentValidation;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserUpdatePasswordCommand : Command<int>
    {
        public long Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public class Validator : CommandValidator<AdminUserUpdatePasswordCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(50).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(50).When(c => c.OldPassword != c.NewPassword).WithName("新密码");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdatePasswordCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<int> Handle(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var admin = await db.Context.AdminUser.FindAsync(request.Id);

                request.NewPassword = Encrypt.Md5By32(request.NewPassword);
                request.OldPassword = Encrypt.Md5By32(request.OldPassword);

                if (!admin.Password.Equals(request.OldPassword))
                    throw new Exception("旧密码错误");

                return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword });
            }
        }
    }
}
