using DDD.Domain.Common.Interfaces;
using DDD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbContext : DBContextFactory, INigelDbContext
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options) { }

        public DbSet<AdminUserEntity> AdminUser => Set<AdminUserEntity>();
    }
}
