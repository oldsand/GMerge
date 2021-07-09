using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using GalaxyMerge.Primitives.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class QueuedEntryConfiguration : IEntityTypeConfiguration<QueuedEntry>
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