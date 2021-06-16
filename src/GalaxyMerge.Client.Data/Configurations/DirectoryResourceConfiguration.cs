using GalaxyMerge.Client.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Client.Data.Configurations
{
    public class DirectoryResourceConfiguration : IEntityTypeConfiguration<DirectoryResource>
    {
        public void Configure(EntityTypeBuilder<DirectoryResource> builder)
        {
            builder.ToTable("Directory").HasKey(x => x.ResourceId);
            builder.Property(x => x.DirectoryName).IsRequired();

            builder.HasOne(x => x.Resource).WithOne(x => x.Directory)
                .HasForeignKey<DirectoryResource>(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}