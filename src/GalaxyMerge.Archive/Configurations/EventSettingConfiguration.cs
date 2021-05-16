using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class EventSettingConfiguration : IEntityTypeConfiguration<EventSetting>
    {
        public void Configure(EntityTypeBuilder<EventSetting> builder)
        {
            builder.ToTable("EventSetting").HasKey(x => x.EventId);
            
            builder.Property(x => x.OperationId).IsRequired();
            builder.Property(x => x.OperationName).IsRequired();
            builder.Property(x => x.OperationType).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<OperationType>(x));
            builder.Property(x => x.IsArchiveTrigger).IsRequired();
            
            builder.HasIndex(x => x.OperationId).IsUnique();
            builder.HasIndex(x => x.OperationName).IsUnique();
            
            builder.Ignore(x => x.Operation);
        }
    }
}