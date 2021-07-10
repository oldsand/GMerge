using GCommon.Core;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    internal class EventSettingConfiguration : IEntityTypeConfiguration<EventSetting>
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