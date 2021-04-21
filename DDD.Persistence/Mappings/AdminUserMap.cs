using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserMap : EntityTypeConfiguration<AdminUserEntity>
    {
        public AdminUserMap() : base("AdminUsers", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }

        public override void Configure(EntityTypeBuilder<AdminUserEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
