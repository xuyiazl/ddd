using DDD.Domain.AdminUsers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Interfaces
{
    public interface IAdminUserAppService
    {
        Task<Result<int>> CreateAsync(AdminUserCommand.CreateCommand command, CancellationToken cancellationToken);
        Task<Result<int>> DeleteAsync(long id, CancellationToken cancellationToken);
        Task<Result<AdminUserDto>> GetAsync(long id, CancellationToken cancellationToken);
        Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken);
        Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken);
        Task<Result<int>> UpdateAsync(AdminUserCommand.UpdateCommand command, CancellationToken cancellationToken);
    }
}