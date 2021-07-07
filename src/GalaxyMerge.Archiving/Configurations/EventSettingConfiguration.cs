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
            
            builder.Property(x => x.Operation)
                .HasConversion(x => x.Name, x => Enumeration.FromName<Operation>(x))
                .IsRequired();
            builder.Property(x => x.OperationType)
                .HasConversion(x => x.Name, x => Enumeration.FromName<OperationType>(x))
                .IsRequired();
            builder.Property(x => x.IsArchiveEvent).IsRequired();
            
            builder.HasIndex(x => x.Operation).IsUnique();

            builder.HasOne(x => x.Archive).WithMany(x => x.EventSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}