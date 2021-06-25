using System;
using GalaxyMerge.Client.Data.Entities;
using GalaxyMerge.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Client.Data.Configurations
{
    public class ResourceEntryConfiguration : IEntityTypeConfiguration<ResourceEntry>
    {
        public void Configure(EntityTypeBuilder<ResourceEntry> builder)
        {
            builder.ToTable("Resource").HasKey(x => x.ResourceId);
            
            builder.Property(x => x.ResourceName).IsRequired();
            builder.Property(x => x.ResourceType).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<ResourceType>(x));
            builder.Property(x => x.AddedOn).IsRequired();
            builder.Property(x => x.AddedBy).IsRequired();
            
            builder.HasIndex(x => x.ResourceName).IsUnique();
        }
    }
}