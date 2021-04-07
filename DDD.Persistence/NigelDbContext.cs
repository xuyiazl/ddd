using DDD.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbContext : DBContextFactory, INigelDbContext
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options) { }
    }
}
