using GCommon.Core;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving.Entities;
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