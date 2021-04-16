using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveInfoConfiguration : IEntityTypeConfiguration<ArchiveInfo>
    {
        public void Configure(EntityTypeBuilder<ArchiveInfo> builder)
        {
            builder.HasKey(x => x.GalaxyName);
            builder.Property(x => x.GalaxyName).IsRequired();
            builder.Property(x => x.VersionNumber).IsRequired();
            builder.Property(x => x.IsaVersion).IsRequired();
            builder.Property(x => x.CdiVersion).IsRequired();
        }
    }
}