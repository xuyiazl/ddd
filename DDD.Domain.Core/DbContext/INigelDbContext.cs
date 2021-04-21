using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.Core
{
    public interface INigelDbContext : IDbContext
    {
        DbSet<AdminUserEntity> AdminUser { get; }
    }
}
