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

namespace DDD.Domain.AdminUsers.Commands.Delete
{
    public class DeleteAdminUserCommand : IRequest<Result<int>>
    {
        public long Id { get; set; }

        public class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand, Result<int>>
        {
            private readonly INigelDbRepository<AdminUser> repository;
            private readonly IMediator mediator;

            public DeleteAdminUserCommandHandler(INigelDbRepository<AdminUser> repository, IMediator mediator)
            {
                this.repository = repository;
                this.mediator = mediator;
            }

            public async Task<Result<int>> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
            {
                var has = await repository.AnyAsync(c => c.Id == request.Id);

                if (!has)
                    return Return.Fail(SubCode.Undefind, 0);

                var res = await repository.DeleteAsync(c => c.Id == request.Id);

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
