using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class ChangeLogInfoConfiguration : IEntityTypeConfiguration<ChangeLogInfo>
    {
        public void Configure(EntityTypeBuilder<ChangeLogInfo> builder)
        {
            builder.ToTable(nameof(ChangeLogInfo)).HasKey(x => x.ChangeLogId);

            builder.Property(x => x.EntryId).IsRequired();
            
            builder.HasIndex(x => x.EntryId).IsUnique();

            builder.HasOne(x => x.Entry)
                .WithOne(x => x.ChangeLog)
                .HasForeignKey<ChangeLogInfo>(x => x.EntryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}