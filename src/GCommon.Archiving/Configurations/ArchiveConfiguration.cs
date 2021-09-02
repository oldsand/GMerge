using GCommon.Primitives;
using GCommon.Primitives.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    internal class ArchiveConfiguration : IEntityTypeConfiguration<ArchiveConfig>
    {
        public void Configure(EntityTypeBuilder<ArchiveConfig> builder)
        {
            builder.ToTable(nameof(ArchiveConfig)).HasKey(x => x.ArchiveId);

            builder.Property(x => x.GalaxyName).IsRequired();
            builder.Property(x => x.Version).HasConversion(x => x.Cdi, x => ArchestraVersion.FromCdi(x));
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired();
        }
    }
}