using GalaxyMerge.Client.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Client.Data.Configurations
{
    public class GalaxyResourceConfiguration : IEntityTypeConfiguration<GalaxyResource>
    {
        public void Configure(EntityTypeBuilder<GalaxyResource> builder)
        {
            builder.ToTable("Resource").HasKey(x => x.ResourceId);
            builder.Property(x => x.ResourceName).IsRequired();
            builder.Property(x => x.ResourceType).IsRequired();
        }
    }
}