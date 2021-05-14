using DDD.Applaction.AdminUsers.Interfaces;
using DDD.Applaction.Common;
using DDD.Domain.AdminUsers;
using DDD.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Services
{
    /// <summary>
    /// 管理员账号管理
    /// </summary>
    [Route("{culture:culture}/api/[controller]/[action]")]
    public class AdminUserAppService : AppService, IAdminUserAppService
    {
        public AdminUserAppService(IMediatorHandler bus, IStringLocalizer<SubCode> localizer) : base(bus, localizer)
        {
        }

        [HttpPost]
        public async Task<Result<int>> CreateAsync([FromBody] AdminUserCreateCommand command, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return Success(SubCode.Success, res);
            else
                return Success(SubCode.Fail, res);
        }

        [HttpPut]
        public async Task<Result<int>> UpdateAsync([FromBody] AdminUserUpdateCommand command, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(command, cancellationToken);

            if (res > 0)
                return Success(SubCode.Success, res);
            else
                return Success(SubCode.Fail, res);
        }

        [HttpDelete("{id:int}")]
        public async Task<Result<int>> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(new AdminUserDeleteCommand { Id = id }, cancellationToken);

            if (res > 0)
                return Success(SubCode.Success, res);
            else
                return Success(SubCode.Fail, res);
        }

        [HttpGet("{id:int}")]
        public async Task<Result<AdminUserDto>> GetAsync(long id, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(new AdminUserQueryById { Id = id }, cancellationToken);

            return Success(SubCode.Success, res);
        }

        [HttpGet]
        public async Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(new AdminUserQueryList { Limit = limit, Keyword = keyword }, cancellationToken);

            return Success(SubCode.Success, res);
        }

        [HttpGet]
        public async Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken)
        {
            var res = await bus.SendCommand(new AdminUserQueryPaged { CurrentPage = currentPage, PageSize = pageSize, Keyword = keyword }, cancellationToken);

            return Success(SubCode.Success, res);
        }
    }
}
