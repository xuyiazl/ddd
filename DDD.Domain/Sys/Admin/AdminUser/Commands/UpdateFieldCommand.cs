﻿using AutoMapper;
using FluentValidation;
using DDD.Domain.Common;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminUser
{
    public class AdminUserUpdateFieldCommand : Command<int>
    {
        public long Id { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }

        public class Validator : CommandValidator<AdminUserUpdateFieldCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateFieldCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Name = request.Value, Updated_At = DateTime.Now });
                    case "username":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { UserName = request.Value, Updated_At = DateTime.Now });
                    case "mobile":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Mobile = request.Value, Updated_At = DateTime.Now });
                    case "password":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Password = Encrypt.Md5By32(request.Value), Updated_At = DateTime.Now });
                    case "position":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Position = request.Value, Updated_At = DateTime.Now });
                    case "location":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Location = request.Value, Updated_At = DateTime.Now });
                    case "company":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Company = request.Value, Updated_At = DateTime.Now });
                    case "picture":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Picture = request.Value, Updated_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
