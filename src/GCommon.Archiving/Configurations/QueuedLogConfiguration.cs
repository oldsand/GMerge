using GCommon.Core;
using GCommon.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class QueuedLogConfiguration : IEntityTypeConfiguration<QueuedLog>
    {
        public void Configure(EntityTypeBuilder<QueuedLog> builder)
        {
            builder.ToTable(nameof(QueuedLog)).HasKey(x => x.ChangeLogId);
            builder.Property(x => x.ObjectId).IsRequired();
            builder.Property(x => x.QueuedOn).IsRequired();
            builder.Property(x => x.ChangedOn).IsRequired();
            builder.Property(x => x.Operation)
                .HasConversion(x => x.Value, x => Operation.FromValue(x))
                .IsRequired();
            builder.Property(x => x.State)
                .HasConversion(x => x.Value, x => ArchiveState.FromValue(x))
                .IsRequired();
        }
    }
}