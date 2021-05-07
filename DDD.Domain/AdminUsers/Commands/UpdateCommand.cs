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
    public class AdminUserUpdateCommand : Command<int>, IMapFrom<AdminUserEntity>
    {
        public long Id { get; set; }
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

        public class Validator : CommandValidator<AdminUserUpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("Id不可为空")
                    .GreaterThan(0).WithMessage(c => $"Id必须大于0");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("名字不可为空")
                    .MaximumLength(20).WithMessage(c => $"名字不能超过20个字符，当前{c.Name.Length}个字符");

                RuleFor(x => x.Picture)
                    .MaximumLength(250).WithMessage(c => $"头像不能超过250个字符，当前{c.Picture.Length}个字符");
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
