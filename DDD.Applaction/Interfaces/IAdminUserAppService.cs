using DDD.Domain.AdminUsers.Commands;
using DDD.Domain.AdminUsers.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.Paging;

namespace DDD.Applaction.AdminUsers.Interfaces
{
    public interface IAdminUserAppService
    {
        Task<Result<int>> CreateAsync(CreateAdminUserCommand command, CancellationToken cancellationToken);
        Task<Result<int>> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<Result<AdminUserDto>> GetAsync(int id, CancellationToken cancellationToken);
        Task<Result<IList<AdminUserDto>>> GetListAsync(int limit, string keyword, CancellationToken cancellationToken);
        Task<Result<PagedModel<AdminUserDto>>> GetPageAsync(int currentPage, int pageSize, string keyword, CancellationToken cancellationToken);
        Task<Result<int>> UpdateAsync(UpdateAdminUserCommand command, CancellationToken cancellationToken);
    }
}