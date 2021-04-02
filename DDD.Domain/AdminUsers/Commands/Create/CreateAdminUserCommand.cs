using DDD.Domain.Common;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.AdminUsers.Commands.Create
{
    public class CreateAdminUserCommand : IRequest<Result<int>>
    {
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }

        public class AddAdminUserCommandHandler : IRequestHandler<CreateAdminUserCommand, Result<int>>
        {
            private readonly INigelDbUnitOfWork unitOfWork;
            private readonly IMediator mediator;

            public AddAdminUserCommandHandler(INigelDbUnitOfWork unitOfWork, IMediator mediator)
            {
                this.unitOfWork = unitOfWork;
                this.mediator = mediator;
            }

            public async Task<Result<int>> Handle(CreateAdminUserCommand request, CancellationToken cancellationToken)
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

                    return Return.Success(SubCode.Success, res);
                }
                else
                    return Return.Success(SubCode.Fail, res);
            }
        }
    }
}
