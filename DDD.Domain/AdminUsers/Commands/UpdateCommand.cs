using AutoMapper;
using DDD.Domain.Common;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserUpdateCommand : CommandId<int>, IMapFrom<AdminUserEntity>
    {
        public string Name { get; set; }
        public string Picture { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserUpdateCommand, AdminUserEntity>()
                .ForMember(c => c.Name, opt => opt.MapFrom(s => s.Name))
                .ForMember(c => c.Picture, opt => opt.MapFrom(s => s.Picture))
                .ForMember(c => c.Updated_At, opt => opt.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandIdValidator<AdminUserUpdateCommand, int>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
                RuleFor(x => x.Picture).MaximumLength(250).WithName("头像");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [RedisCacheRemove(HashKey = RedisKey.Admin, Key = "{Id}")]
            public override async Task<int> Handle(AdminUserUpdateCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Context.AdminUser.Where(c => c.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

                if (entity == null)
                    return 0;

                entity = mapper.Map(request, entity);

                var res = db.Update(entity);

                if (res > 0)

                    await bus.PublishEvent(new UpdateEvent(entity.Id, entity), cancellationToken);

                return res;
            }
        }
    }

}
