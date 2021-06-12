﻿using AutoMapper;
using DDD.Domain.Common;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.AdminMenu
{
    public class AdminMenuCreateCommand : Command<int>, IMapFrom<AdminMenuEntity>
    {
        public long FatherId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string OnlyCode { get; set; }
        public bool IsMenu { get; set; }
        public int Weight { get; set; }
        public bool IsExpress { get; set; }
        public Status Status { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminMenuCreateCommand, AdminMenuEntity>()
                .ForMember(c => c.Url, c => c.MapFrom(s => s.Url.IsEmpty() ? "#" : s.Url))
                .ForMember(c => c.Created_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminMenuCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("菜单名");
                RuleFor(x => x.Url).NotEmpty().MaximumLength(50).WithName("Url");
                RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("唯一代码");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminMenuCreateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminMenuCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<AdminMenuCreateCommand, AdminMenuEntity>(request);

                var res = db.Add(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new CreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}