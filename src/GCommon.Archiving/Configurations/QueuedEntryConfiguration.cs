using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    internal class QueuedEntryConfiguration : IEntityTypeConfiguration<QueuedEntry>
    {
        public void Configure(EntityTypeBuilder<QueuedEntry> builder)
        {
            builder.ToTable(nameof(QueuedEntry)).HasKey(x => x.ChangeLogId);
            builder.Property(x => x.ObjectId).IsRequired();
            builder.Property(x => x.OperationId).IsRequired();
            builder.Property(x => x.ChangedOn).IsRequired();
            builder.Property(x => x.State).HasConversion(x => x.Name, x => Enumeration.FromName<QueueState>(x));
        }
    }
}