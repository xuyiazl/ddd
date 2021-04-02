using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace DDD.Domain.Common.Interfaces
{
    public interface INigelDbRepository<TEntity> : IMsSqlRepository<TEntity> where TEntity : class, new() { }
}
