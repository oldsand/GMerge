using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class EventSettingConfiguration : IEntityTypeConfiguration<EventSetting>
    {
        public void Configure(EntityTypeBuilder<EventSetting> builder)
        {
            builder.ToTable(nameof(EventSetting)).HasKey(x => x.EventId);
            
            builder.Property(x => x.OperationId).IsRequired();
            builder.Property(x => x.OperationType).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<OperationType>(x));
            builder.Property(x => x.IsArchiveEvent).IsRequired();
            
            builder.HasIndex(x => x.OperationId).IsUnique();

            builder.Ignore(x => x.Operation);
            
            builder.HasOne(x => x.Archive).WithMany(x => x.EventSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}