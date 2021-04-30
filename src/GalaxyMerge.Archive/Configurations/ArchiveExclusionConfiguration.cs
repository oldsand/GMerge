using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveExclusionConfiguration : IEntityTypeConfiguration<ArchiveExclusion>
    {
        public void Configure(EntityTypeBuilder<ArchiveExclusion> builder)
        {
            builder.ToTable("Exclusion").HasKey(x => x.ExclusionId);
            builder.Property(x => x.ExclusionName).IsRequired();
        }
    }
}