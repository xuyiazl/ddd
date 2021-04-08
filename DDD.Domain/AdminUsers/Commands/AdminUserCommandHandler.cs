using DDD.Domain.Core;
using DDD.Domain.Core.Bus;
using DDD.Domain.Core.Interfaces;
using DDD.Domain.Entities;
using DDD.Domain.Notifications;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.AdminUsers.Commands
{
    public class AdminUserCommandHandler : CommandHandler,
        IRequestHandler<CreateAdminUserCommand, (SubCode, int)>,
        IRequestHandler<UpdateAdminUserCommand, (SubCode, int)>,
        IRequestHandler<DeleteAdminUserCommand, (SubCode, int)>
    {
        private readonly INigelDbRepository<AdminUser> db;

        public AdminUserCommandHandler(INigelDbRepository<AdminUser> db, IMediatorHandler bus) : base(bus)
        {
            this.db = db;
        }

        public async Task<(SubCode, int)> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
        {
            //await bus.PublishEvent(new DomainNotification("", "开始注册...."), cancellationToken);

            var entity = new AdminUser
            {
                Name = request.Name,
                Company = request.Company,
                CreatedTime = DateTime.Now,
                Location = request.Location,
                LoginCount = 0,
                LoginLastIp = "",
                LoginLastTime = DateTime.Now,
                Mobile = request.Mobile,
                Password = request.Password,
                Picture = request.Picture,
                Position = request.Position,
                Status = true,
                UserName = request.UserName
            };

            db.Add(entity);

            var res = db.UnitOfWork.Commit();

            //await bus.PublishEvent(new DomainNotification("", "结束注册...."), cancellationToken);

            if (res > 0)
            {
                await bus.PublishEvent(new CreateAdminUserEvent(entity.Id, entity), cancellationToken);

                return (SubCode.Success, res);
            }
            else
                return (SubCode.Fail, res);
        }

        public async Task<(SubCode, int)> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await db.GetByIdAsync(request.Id);

            if (entity == null)
                return (SubCode.Undefind, 0);

            entity.Id = request.Id;
            entity.Name = request.Name;
            entity.Company = request.Company;
            entity.Location = request.Location;
            entity.Picture = request.Picture;
            entity.Position = request.Position;

            db.Update(entity);

            var res = db.UnitOfWork.Commit();

            if (res > 0)
            {
                await bus.PublishEvent(new UpdateAdminUserEvent(entity.Id, entity), cancellationToken);

                return (SubCode.Success, res);
            }
            return (SubCode.Fail, res);
        }

        public async Task<(SubCode, int)> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
        {
            var has = await db.AnyAsync(c => c.Id == request.Id);

            if (!has)
                return (SubCode.Undefind, 0);

            var res = await db.DeleteAsync(c => c.Id == request.Id);

            if (res > 0)
            {
                await bus.PublishEvent(new DeleteAdminUserEvent(request.Id), cancellationToken);

                return (SubCode.Success, res);
            }
            return (SubCode.Fail, res);
        }
    }
}
