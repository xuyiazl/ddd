using DDD.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserMap : EntityTypeConfiguration<AdminUser>
    {
        public AdminUserMap() : base("AdminUsers", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }

        public override void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            base.Configure(builder);
        }
    }
}
