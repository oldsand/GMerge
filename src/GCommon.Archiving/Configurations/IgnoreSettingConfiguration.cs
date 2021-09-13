using GCommon.Core;
using GCommon.Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    internal class IgnoreSettingConfiguration : IEntityTypeConfiguration<IgnoreSetting>
    {
        public void Configure(EntityTypeBuilder<IgnoreSetting> builder)
        {
            builder.ToTable(nameof(IgnoreSetting)).HasKey(x => x.IgnoreId);

            builder.Property(x => x.IgnoreType)
                .HasConversion(x => x.Value, x => IgnoreType.FromValue(x))
                .IsRequired();

            builder.Property(x => x.Template)
                .HasConversion(x => x.Value, x => Template.FromValue(x));

            builder.HasOne(x => x.ArchiveConfig).WithMany(x => x.IgnoreSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}