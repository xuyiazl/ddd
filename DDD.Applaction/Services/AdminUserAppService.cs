using DDD.Applaction.Common;
using DDD.Domain.AdminUsers.Commands;
using DDD.Domain.AdminUsers.Dtos;
using DDD.Domain.AdminUsers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Services
{
    /// <summary>
    /// 管理员账号管理
    /// </summary>
    public class AdminUserAppService : AppService
    {
        public AdminUserAppService(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<Result<int>> CreateAsync(CreateAdminUserCommand command, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(command, cancellationToken);

            return Success(subCode, res);
        }

        [HttpPut]
        public async Task<Result<int>> UpdateAsync(UpdateAdminUserCommand command, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(command, cancellationToken);

            return Success(subCode, res);
        }

        [HttpDelete("{id:int}")]
        public async Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(new DeleteAdminUserCommand { Id = id }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet("{id:int}")]
        public async Task<Result<AdminUserDto>> GetAsync(int id, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(new AdminUserDetailQuery { Id = id }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet]
        public async Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(new AdminUserListQuery { Limit = limit, Keyword = keyword }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet]
        public async Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken)
        {
            var (subCode, res) = await mediator.Send(new AdminUserPagedListQuery { CurrentPage = currentPage, PageSize = pageSize, Keyword = keyword }, cancellationToken);

            return Success(subCode, res);
        }
    }
}
