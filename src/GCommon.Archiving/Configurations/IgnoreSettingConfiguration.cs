using GCommon.Core;
using GCommon.Primitives;
using GCommon.Primitives.Base;
using GCommon.Archiving.Entities;
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
                .HasConversion(x => x.Name, x => Enumeration.FromName<IgnoreType>(x))
                .IsRequired();

            builder.Property(x => x.Template)
                .HasConversion(x => x.Name, x => Enumeration.FromName<Template>(x));

            builder.HasOne(x => x.Archive).WithMany(x => x.IgnoreSettings).HasForeignKey(x => x.ArchiveId);
        }
    }
}