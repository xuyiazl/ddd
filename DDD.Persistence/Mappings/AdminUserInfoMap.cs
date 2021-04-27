using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserInfoMap : BaseMapping<AdminUserInfoEntity>
    {
        public AdminUserInfoMap() : base("AdminUserInfo", t => t.Id) { }

        public override void Configure(EntityTypeBuilder<AdminUserInfoEntity> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.UserId).IsRequired().HasColumnType("bigint");
            builder.Property(c => c.Sex).IsRequired().HasColumnType("int");
            builder.Property(c => c.Address).IsRequired().HasColumnType("varchar(50)");

            builder.HasOne(d => d.User)
              .WithOne(p => p.UserInfo)
              .HasForeignKey<AdminUserInfoEntity>(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminUser_AdminUserInfo");
        }
    }
}
