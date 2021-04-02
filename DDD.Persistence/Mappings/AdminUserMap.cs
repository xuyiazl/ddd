using DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserMap : EntityTypeConfiguration<AdminUser>
    {
        public AdminUserMap() : base("AdminUsers", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }
    }
}
