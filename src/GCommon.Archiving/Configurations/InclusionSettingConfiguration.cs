using GCommon.Core;
using GCommon.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    internal class InclusionSettingConfiguration : IEntityTypeConfiguration<InclusionSetting>
    {
        public void Configure(EntityTypeBuilder<InclusionSetting> builder)
        {
            builder.ToTable(nameof(InclusionSetting)).HasKey(x => x.InclusionId);

            builder.Property(x => x.Template)
                .HasConversion(x => x.Value, x => Template.FromValue(x))
                .IsRequired();
            builder.Property(x => x.InclusionOption)
                .HasConversion(x => x.Value, x => InclusionOption.FromValue(x))
                .IsRequired();
            builder.Property(x => x.IncludeInstances).IsRequired();

            builder.HasIndex(x => x.Template).IsUnique();

            builder.HasOne(x => x.ArchiveConfig).WithMany(x => x.InclusionSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}