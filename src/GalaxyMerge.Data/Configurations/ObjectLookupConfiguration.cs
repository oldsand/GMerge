using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class ObjectLookupConfiguration : IEntityTypeConfiguration<ObjectLookup>
    {
        public void Configure(EntityTypeBuilder<ObjectLookup> builder)
        {
            builder.HasKey(x => x.ObjectId);
            builder.Property(x => x.ObjectId).HasColumnName("gobject_id");
            builder.Property(x => x.DerivedFromId).HasColumnName("derived_from_gobject_id");
            builder.Property(x => x.TagName).HasColumnName("tag_name");
        }
    }
}