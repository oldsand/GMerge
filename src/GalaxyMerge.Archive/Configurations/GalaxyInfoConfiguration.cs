using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    internal class GalaxyInfoConfiguration : IEntityTypeConfiguration<GalaxyInfo>
    {
        public void Configure(EntityTypeBuilder<GalaxyInfo> builder)
        {
            builder.ToTable(nameof(GalaxyInfo)).HasKey(x => x.GalaxyName);
        }
    }
}