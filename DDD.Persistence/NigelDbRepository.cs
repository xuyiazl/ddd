using DDD.Domain.Core.Interfaces;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
