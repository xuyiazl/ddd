using AutoMapper;
using DDD.Domain.Common.Mappings;
using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace DDD.Domain.AdminUsers
{
    public class AdminUserCreateCommand : Command<int>, IMapFrom<AdminUserEntity>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserCreateCommand, AdminUserEntity>()
                .ForMember(c => c.Status, opt => opt.MapFrom(s => Status.Show))
                .ForMember(c => c.Created_At, opt => opt.MapFrom(s => DateTime.Now))
            ;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminUserCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserName).NotEmpty().MaximumLength(20).WithName("账号");
                RuleFor(x => x.Password).NotEmpty().MaximumLength(30).WithName("密码");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
                RuleFor(x => x.Picture).MaximumLength(250).WithName("头像");
            }
        }

        public class Handler : CommandHandler<AdminUserCreateCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus, mapper)
            {
                this.db = db;
            }

            public override async Task<int> Handle(AdminUserCreateCommand request, CancellationToken cancellationToken)
            {
                //await bus.PublishEvent(new DomainNotification("", "开始注册...."), cancellationToken);

                var entity = mapper.Map<AdminUserCreateCommand, AdminUserEntity>(request);

                var res = db.Add(entity);

                //await bus.PublishEvent(new DomainNotification("", "结束注册...."), cancellationToken);

                if (res > 0)

                    await bus.PublishEvent(new CreateEvent(entity.Id, entity), cancellationToken);

                return res;
            }
        }
    }
}
