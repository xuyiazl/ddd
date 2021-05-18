using DDD.Domain.AdminUsers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Interfaces
{
    public interface IAdminUserAppService
    {
        Task<Result<int>> CreateAsync(AdminUserCreateCommand command, CancellationToken cancellationToken);
        Task<Result<int>> DeleteAsync(long id, CancellationToken cancellationToken);
        Task<Result<AdminUserDto>> GetAsync(long id, CancellationToken cancellationToken);
        Task<Result<IList<AdminUserDto>>> GetListAsync(AdminUserQueryList query, CancellationToken cancellationToken);
        Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(AdminUserQueryPaged query, CancellationToken cancellationToken);
        Task<Result<int>> UpdateAsync(AdminUserUpdateCommand command, CancellationToken cancellationToken);
    }
}