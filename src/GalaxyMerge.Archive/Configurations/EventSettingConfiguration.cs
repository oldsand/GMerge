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
            builder.ToTable(nameof(EventSetting)).HasKey(x => x.OperationId);
            
            builder.Property(x => x.OperationType).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<OperationType>(x));
            builder.Property(x => x.IsArchiveTrigger).IsRequired();

            builder.Ignore(x => x.Operation);
        }
    }
}