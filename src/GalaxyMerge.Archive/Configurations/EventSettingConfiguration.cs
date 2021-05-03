using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Enum;
using GalaxyMerge.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class EventSettingConfiguration : IEntityTypeConfiguration<EventSetting>
    {
        public void Configure(EntityTypeBuilder<EventSetting> builder)
        {
            builder.ToTable("EventSetting").HasKey(x => x.EventId);
            builder.Property(x => x.EventName).IsRequired();
            builder.Property(x => x.EventType).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<EventType>(x));
        }
    }
}