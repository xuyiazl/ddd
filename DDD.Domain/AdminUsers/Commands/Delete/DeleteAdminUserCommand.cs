using DDD.Domain.Common;
using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.AdminUsers.Commands.Delete
{
    public class DeleteAdminUserCommand : IRequest<Result<int>>
    {
        public long Id { get; set; }

        public class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand, Result<int>>
        {
            private readonly INigelDbUnitOfWork unitOfWork;
            private readonly IMediator mediator;

            public DeleteAdminUserCommandHandler(INigelDbUnitOfWork unitOfWork, IMediator mediator)
            {
                this.unitOfWork = unitOfWork;
                this.mediator = mediator;
            }

            public async Task<Result<int>> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
            {
                var has = await unitOfWork.AnyAsync<AdminUser>(c => c.Id == request.Id);

                if (!has)
                    return Return.Fail(SubCode.Undefind, 0);

                var res = await unitOfWork.DeleteAsync<AdminUser>(c => c.Id == request.Id);

                if (res > 0)
                {
                    await mediator.Publish(new DeleteAdminUserEvent { Id = request.Id }, cancellationToken);

                    return Return.Success(SubCode.Success, res);
                }
                return Return.Fail(SubCode.Fail, res);
            }
        }
    }
}
