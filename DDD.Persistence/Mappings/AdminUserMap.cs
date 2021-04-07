using DDD.Domain.Entities;
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
