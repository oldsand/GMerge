using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Common.Primitives;
using GalaxyMerge.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable("InclusionSetting").HasKey(x => x.InclusionId);
            
            builder.Property(x => x.TemplateId).IsRequired();
            builder.Property(x => x.TemplateName).IsRequired();
            builder.Property(x => x.InclusionOption).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<InclusionOption>(x));
            builder.Property(x => x.IncludesInstances).IsRequired();
            
            builder.HasIndex(x => x.TemplateId).IsUnique();
            builder.HasIndex(x => x.TemplateName).IsUnique();
            
            builder.Ignore(x => x.Template);
        }
    }
}