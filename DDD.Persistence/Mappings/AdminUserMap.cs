using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserMap : BaseMapping<AdminUserEntity>
    {
        public AdminUserMap() : base("AdminUser", t => t.Id) { }

        public override void Configure(EntityTypeBuilder<AdminUserEntity> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.Name).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.UserName).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.Password).IsRequired().HasColumnType("varchar(50)");
            builder.Property(c => c.Picture).IsRequired().HasColumnType("varchar(250)");
        }
    }
}
