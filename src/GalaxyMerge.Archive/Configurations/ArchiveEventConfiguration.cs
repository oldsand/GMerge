using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveEventConfiguration : IEntityTypeConfiguration<ArchiveEvent>
    {
        public void Configure(EntityTypeBuilder<ArchiveEvent> builder)
        {
            builder.ToTable("Event").HasKey(x => x.EventId);
            builder.Property(x => x.EventName).IsRequired();
            builder.Property(x => x.IsCreationEvent).IsRequired();
        }
    }
}