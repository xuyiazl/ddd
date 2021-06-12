﻿using AutoMapper;
using DDD.Domain.Common;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminRole
{
    public class AdminRoleUpdateCommand : Command<int>, IMapFrom<AdminRoleEntity>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long[] MenuIds { get; set; }
        public Status Status { get; set; }


        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminRoleUpdateCommand, AdminRoleEntity>()
                .ForMember(c => c.Updated_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminRoleUpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminRoleUpdateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }


            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleUpdateCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Context.AdminAuthRole.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (entity == null)
                    return 0;

                entity = mapper.Map(request, entity);

                //先清空导航集合，确保没有冗余信息
                await db.DeleteAsync<AdminRoleMenuEntity>(c => c.RoleId == entity.Id);

                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new AdminRoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                var res = db.Update(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new UpdateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                return res;
            }
        }
    }
}