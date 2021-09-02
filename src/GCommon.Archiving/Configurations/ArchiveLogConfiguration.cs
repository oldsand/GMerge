using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Primitives.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class ArchiveLogConfiguration : IEntityTypeConfiguration<ArchiveLog>
    {
        public void Configure(EntityTypeBuilder<ArchiveLog> builder)
        {
            builder.ToTable(nameof(ArchiveLog)).HasKey(x => x.EntryId);

            builder.Property(x => x.EntryId).ValueGeneratedNever();
            builder.Property(x => x.ChangeLogId).IsRequired();
            builder.Property(x => x.ObjectId).IsRequired();
            builder.Property(x => x.ChangedOn).IsRequired();
            builder.Property(x => x.Operation)
                .HasConversion(x => x.Name, x => Operation.FromName(x, false))
                .IsRequired();
            builder.Property(x => x.Comment).IsRequired();
            builder.Property(x => x.UserName).IsRequired();

            builder.HasIndex(x => x.ChangeLogId).IsUnique();

            builder.HasOne(x => x.Entry).WithOne(x => x.Log).HasForeignKey<ArchiveLog>(x => x.EntryId);
        }
    }
}