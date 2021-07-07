using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Core;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    public class IgnoreSettingConfiguration : IEntityTypeConfiguration<IgnoreSetting>
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