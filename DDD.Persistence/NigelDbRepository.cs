using DDD.Domain.Core;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbRepository : Repository<INigelDbContext>, INigelDbRepository
    {
        public NigelDbRepository(INigelDbContext context) : base(context) { }
    }
}
