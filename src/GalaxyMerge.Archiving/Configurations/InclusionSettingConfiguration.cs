using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using GalaxyMerge.Primitives.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable(nameof(InclusionSetting)).HasKey(x => x.InclusionId);

            builder.Property(x => x.Template)
                .HasConversion(x => x.Name, x => Enumeration.FromName<Template>(x))
                .IsRequired();
            builder.Property(x => x.InclusionOption)
                .HasConversion(x => x.Name, x => Enumeration.FromName<InclusionOption>(x))
                .IsRequired();
            builder.Property(x => x.IncludeInstances).IsRequired();

            builder.HasIndex(x => x.Template).IsUnique();

            builder.HasOne(x => x.Archive).WithMany(x => x.InclusionSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}