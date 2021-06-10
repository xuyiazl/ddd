using AutoMapper;
using FluentValidation;
using DDD.Domain.Common;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.Sys.LoginRecord
{
    public class LoginRecordCreateCommand : Command<int>, IMapFrom<LoginRecordEntity>
    {
        public long AdminId { get; set; }
        public string LoginWay { get; set; }
        public DateTime LoginTime { get; set; }
        public string LoginIp { get; set; }


        public void Mapping(Profile profile) =>
            profile.CreateMap<LoginRecordCreateCommand, LoginRecordEntity>()
                .ForMember(c => c.LoginTime, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<LoginRecordCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
                RuleFor(x => x.LoginWay).NotEmpty().WithName("登录方式");
                RuleFor(x => x.LoginIp).NotEmpty().MaximumLength(20).WithName("登录IP");
            }
        }

        public class Handler : CommandHandler<LoginRecordCreateCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus, mapper)
            {
                this.db = db;
            }


            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(LoginRecordCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<LoginRecordCreateCommand, LoginRecordEntity>(request);

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
