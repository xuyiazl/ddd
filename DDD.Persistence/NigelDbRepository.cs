using DDD.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbRepository<TEntity> : MsSqlRepository<TEntity>, INigelDbRepository<TEntity> where TEntity : class, new()
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
