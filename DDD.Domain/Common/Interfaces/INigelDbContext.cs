using DDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.Common.Interfaces
{
    public interface INigelDbContext : IDbContext
    {
        DbSet<AdminUserEntity> AdminUser { get; }
    }
}
