
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.Core.Interfaces
{
    public interface INigelDbRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
}
