using DDD.Domain.Core;
using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbContext : DBContextFactory, INigelDbContext
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options)
        {

        }

        public override Assembly[] Assemblies => new Assembly[] { Assembly.GetExecutingAssembly() };

        public DbSet<AdminUserEntity> AdminUser => Set<AdminUserEntity>();
        public DbSet<AdminUserInfoEntity> AdminUserInfo => Set<AdminUserInfoEntity>();
        public DbSet<AdminUserLoginRecordEntity> AdminUserLoginRecord => Set<AdminUserLoginRecordEntity>();
    }
}