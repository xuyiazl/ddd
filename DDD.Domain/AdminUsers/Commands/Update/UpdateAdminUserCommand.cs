﻿using DDD.Domain.Common;
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

namespace DDD.Domain.AdminUsers.Commands.Update
{
    public class UpdateAdminUserCommand : IRequest<Result<int>>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Location { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, Result<int>>
        {
            private readonly INigelDbRepository<AdminUser> repository;
            private readonly IMediator mediator;

            public UpdateAdminUserCommandHandler(INigelDbRepository<AdminUser> repository, IMediator mediator)
            {
                this.repository = repository;
                this.mediator = mediator;
            }

            public async Task<Result<int>> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
            {
                var entity = await repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return ResultModel.Fail(SubCode.Undefind, 0);

                entity.Id = request.Id;
                entity.Name = request.Name;
                entity.Company = request.Company;
                entity.Location = request.Location;
                entity.Picture = request.Picture;
                entity.Position = request.Position;

                var res = await repository.UpdateAsync(entity, cancellationToken: cancellationToken);

                if (res > 0)
                {
                    await mediator.Publish(new UpdateAdminUserEvent { User = entity }, cancellationToken);

                    return ResultModel.Success(SubCode.Success, res);
                }
                return ResultModel.Fail(SubCode.Fail, res);
            }
        }
    }
}