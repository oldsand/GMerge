using GalaxyMerge.Archive.Entities;
using GalaxyMerge.Archive.Enum;
using GalaxyMerge.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable("InclusionSetting").HasKey(x => x.TemplateId);
            builder.Property(x => x.TemplateName).IsRequired();
            builder.Property(x => x.InclusionOption).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<InclusionOption>(x));
        }
    }
}