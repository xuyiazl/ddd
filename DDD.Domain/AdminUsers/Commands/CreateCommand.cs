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
                //RuleFor(x => x.Id).Length(5).NotEmpty();
                RuleFor(x => x.UserName)
                    .NotEmpty().WithMessage("账号不可为空")
                    .MaximumLength(20).WithMessage(c => $"账号不能超过20个字符，当前{c.UserName.Length}个字符");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("密码不可为空")
                    .MaximumLength(30).WithMessage(c => $"密码不能超过30个字符，当前{c.Password.Length}个字符");

                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("名字不可为空")
                    .MaximumLength(20).WithMessage(c => $"名字不能超过20个字符，当前{c.Name.Length}个字符");

                RuleFor(x => x.Picture)
                    .MaximumLength(250).WithMessage(c => $"头像不能超过250个字符，当前{c.Picture.Length}个字符");
            }
        }

        public class Handler : CommandHandler<AdminUserCreateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
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
