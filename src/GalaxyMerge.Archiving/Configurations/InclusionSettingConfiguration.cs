using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable(nameof(InclusionSetting)).HasKey(x => x.InclusionId);

            builder.Property(x => x.TemplateId).IsRequired();
            builder.Property(x => x.InclusionOption).IsRequired()
                .HasConversion(x => x.Name, x => Enumeration.FromName<InclusionOption>(x));
            builder.Property(x => x.IncludeInstances).IsRequired();

            builder.HasIndex(x => x.TemplateId).IsUnique();
            
            builder.Ignore(x => x.Template);

            builder.HasOne(x => x.Archive).WithMany(x => x.InclusionSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}