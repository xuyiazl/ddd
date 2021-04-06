using DDD.Domain.AdminUsers.Commands.Event;
using DDD.Domain.Common;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.AdminUsers.Commands
{
    public class AdminUserCommandHandler :
        IRequestHandler<CreateAdminUserCommand, (SubCode, int)>,
        IRequestHandler<UpdateAdminUserCommand, (SubCode, int)>,
        IRequestHandler<DeleteAdminUserCommand, (SubCode, int)>
    {
        private readonly INigelDbUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public AdminUserCommandHandler(INigelDbUnitOfWork unitOfWork, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mediator = mediator;
        }

        public async Task<(SubCode, int)> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
        {
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

            unitOfWork.Add(entity);

            var res = unitOfWork.Commit();

            if (res > 0)
            {
                await mediator.Publish(new CreateAdminUserEvent { User = entity }, cancellationToken);

                return (SubCode.Success, res);
            }
            else
                return (SubCode.Fail, res);
        }

        public async Task<(SubCode, int)> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
        {
            var entity = await unitOfWork.GetByIdAsync<AdminUser>(request.Id);

            if (entity == null)
                return (SubCode.Undefind, 0);

            entity.Id = request.Id;
            entity.Name = request.Name;
            entity.Company = request.Company;
            entity.Location = request.Location;
            entity.Picture = request.Picture;
            entity.Position = request.Position;

            unitOfWork.Update(entity);

            var res = unitOfWork.Commit();

            if (res > 0)
            {
                await mediator.Publish(new UpdateAdminUserEvent { User = entity }, cancellationToken);

                return (SubCode.Success, res);
            }
            return (SubCode.Fail, res);
        }

        public async Task<(SubCode, int)> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
        {
            var has = await unitOfWork.AnyAsync<AdminUser>(c => c.Id == request.Id);

            if (!has)
                return (SubCode.Undefind, 0);

            var res = await unitOfWork.DeleteAsync<AdminUser>(c => c.Id == request.Id);

            if (res > 0)
            {
                await mediator.Publish(new DeleteAdminUserEvent { Id = request.Id }, cancellationToken);

                return (SubCode.Success, res);
            }
            return (SubCode.Fail, res);
        }
    }
}
