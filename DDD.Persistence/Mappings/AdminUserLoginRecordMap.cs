using DDD.Domain.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XUCore.NetCore.Data.DbService;

namespace DDD.Persistence.Mappings
{
    public class AdminUserLoginRecordMap : EntityTypeConfiguration<AdminUserLoginRecordEntity>
    {
        public AdminUserLoginRecordMap() : base("AdminUserLoginRecord", t => t.Id)
        {
            SetIndentity(t => t.Id);
        }

        public override void Configure(EntityTypeBuilder<AdminUserLoginRecordEntity> builder)
        {
            base.Configure(builder);

            builder.Property(c => c.UserId).IsRequired().HasColumnType("bigint");
            builder.Property(c => c.Mode).IsRequired().HasColumnType("varchar(10)");
            builder.Property(c => c.LoginTime).IsRequired().HasColumnType("datetime");

            builder.HasOne(d => d.User)
              .WithMany(p => p.LoginRecords)
              .HasForeignKey(d => d.UserId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FK_AdminUser_AdminUserLoginRecord");
        }
    }
}
