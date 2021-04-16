using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.Common;
using DDD.Domain.AdminUsers;
using DDD.Domain.Core.Bus;
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
    public class AdminUserAppService : AppService, IAdminUserAppService
    {
        public AdminUserAppService(IMediatorHandler bus) : base(bus) { }

        [HttpPost]
        public async Task<Result<int>> CreateAsync(AdminUserCommand.CreateCommand command, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(command, cancellationToken);

            return Success(subCode, res);
        }

        [HttpPut]
        public async Task<Result<int>> UpdateAsync(AdminUserCommand.UpdateCommand command, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(command, cancellationToken);

            return Success(subCode, res);
        }

        [HttpDelete("{id:int}")]
        public async Task<Result<int>> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(new AdminUserCommand.DeleteCommand { Id = id }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet("{id:int}")]
        public async Task<Result<AdminUserDto>> GetAsync(long id, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(new AdminUserCommand.QueryDetail { Id = id }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet]
        public async Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(new AdminUserCommand.QueryList { Limit = limit, Keyword = keyword }, cancellationToken);

            return Success(subCode, res);
        }

        [HttpGet]
        public async Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken)
        {
            var (subCode, res) = await bus.SendCommand(new AdminUserCommand.QueryPaged { CurrentPage = currentPage, PageSize = pageSize, Keyword = keyword }, cancellationToken);

            return Success(subCode, res);
        }
    }
}
