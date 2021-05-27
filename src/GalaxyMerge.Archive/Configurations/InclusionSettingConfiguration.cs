using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable(nameof(InclusionSetting)).HasKey(x => x.TemplateId);
            
            builder.Property(x => x.InclusionOption).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<InclusionOption>(x));
            builder.Property(x => x.IncludesInstances).IsRequired();

            builder.Ignore(x => x.Template);
        }
    }
}