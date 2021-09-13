using System;
using GClient.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GClient.Data.Configurations
{
    public class ResourceEntryConfiguration : IEntityTypeConfiguration<ResourceEntry>
    {
        public void Configure(EntityTypeBuilder<ResourceEntry> builder)
        {
            builder.ToTable("Resource").HasKey(x => x.ResourceId);
            
            builder.Property(x => x.ResourceName).IsRequired();
            builder.Property(x => x.ResourceType)
                .HasConversion(x => x.ToString(), x => (ResourceType) Enum.Parse(typeof(ResourceType), x))
                .IsRequired();
            builder.Property(x => x.AddedOn).IsRequired();
            builder.Property(x => x.AddedBy).IsRequired();
            
            builder.HasIndex(x => x.ResourceName).IsUnique();
        }
    }
}