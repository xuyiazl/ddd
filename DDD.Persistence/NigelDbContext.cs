using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbContext : DBContextFactory, INigelDbContext
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options)
        {
            base.Database.Migrate();
        }

        public DbSet<AdminUserEntity> AdminUser => Set<AdminUserEntity>();
    }
}