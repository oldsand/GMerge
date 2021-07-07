using GalaxyMerge.Archiving.Entities;
using GalaxyMerge.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archiving.Configurations
{
    internal class ArchiveConfiguration : IEntityTypeConfiguration<Archive>
    {
        public void Configure(EntityTypeBuilder<Archive> builder)
        {
            builder.ToTable(nameof(Archive)).HasKey(x => x.ArchiveId);

            builder.Property(x => x.GalaxyName).IsRequired();
            builder.Property(x => x.Version).HasConversion(x => x.Cdi, x => ArchestraVersion.FromCid(x));
            builder.Property(x => x.CreatedOn).IsRequired();
            builder.Property(x => x.UpdatedOn).IsRequired();
        }
    }
}