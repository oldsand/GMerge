using GCommon.Archiving.Entities;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class ArchiveLogConfiguration : IEntityTypeConfiguration<ArchiveLog>
    {
        public void Configure(EntityTypeBuilder<ArchiveLog> builder)
        {
            builder.ToTable(nameof(ArchiveLog)).HasKey(x => x.ChangeLogId);

            builder.Property(x => x.ChangeLogId).ValueGeneratedNever();
            builder.Property(x => x.ObjectId).IsRequired();
            builder.Property(x => x.ChangedOn).IsRequired();
            builder.Property(x => x.Operation)
                .HasConversion(x => x.Name, x => Enumeration.FromName<Operation>(x))
                .IsRequired();
            builder.Property(x => x.State)
                .HasConversion(x => x.Name, x => Enumeration.FromName<ArchiveState>(x))
                .IsRequired();

            builder.Ignore(x => x.Entry);
        }
    }
}