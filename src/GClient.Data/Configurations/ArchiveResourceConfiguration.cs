using GClient.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GClient.Data.Configurations
{
    public class ArchiveResourceConfiguration : IEntityTypeConfiguration<ArchiveResource>
    {
        public void Configure(EntityTypeBuilder<ArchiveResource> builder)
        {
            builder.ToTable("Archive").HasKey(x => x.ResourceId);
            builder.Property(x => x.FileName).IsRequired();

            builder.HasOne(x => x.Resource).WithOne(x => x.Archive)
                .HasForeignKey<ArchiveResource>(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}