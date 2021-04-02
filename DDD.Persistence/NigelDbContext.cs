using DDD.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence
{
    public class NigelDbContext : DBContextFactory, INigelDbContext
    {
        public NigelDbContext(DbContextOptions<NigelDbContext> options) : base(options) { }
    }
}
