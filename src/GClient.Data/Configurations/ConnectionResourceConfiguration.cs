using GClient.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GClient.Data.Configurations
{
    public class ConnectionResourceConfiguration : IEntityTypeConfiguration<ConnectionResource>
    {
        public void Configure(EntityTypeBuilder<ConnectionResource> builder)
        {
            builder.ToTable("Connection").HasKey(x => x.ResourceId);
            builder.Property(x => x.NodeName).IsRequired();
            builder.Property(x => x.GalaxyName).IsRequired();

            builder.HasOne(x => x.Resource).WithOne(x => x.Connection)
                .HasForeignKey<ConnectionResource>(x => x.ResourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}