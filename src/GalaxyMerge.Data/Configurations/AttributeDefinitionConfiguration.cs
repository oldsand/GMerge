using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class AttributeDefinitionConfiguration : IEntityTypeConfiguration<AttributeDefinition>
    {
        public void Configure(EntityTypeBuilder<AttributeDefinition> builder)
        {
            builder.ToView("attribute_definition").HasKey(a => a.AttributeDefinitionId);
            
            builder.Property(a => a.PrimitiveDefinitionId).HasColumnName("primitive_definition_id");
            builder.Property(a => a.AttributeName).HasColumnName("attribute_name");
            builder.Property(a => a.AttributeId).HasColumnName("mx_attribute_id");
            builder.Property(a => a.HasConfigSetHandler).HasColumnName("has_config_set_handler");
            builder.Property(a => a.DataTypeId).HasColumnName("mx_data_type");
            builder.Property(a => a.SecurityClassificationId).HasColumnName("security_classification");
            builder.Property(a => a.AttributeCategoryId).HasColumnName("mx_attribute_category");
            builder.Property(a => a.IsLocked).HasColumnName("is_locked");
            builder.Property(a => a.RawValue).HasColumnName("mx_value");
            builder.Property(a => a.IsArray).HasColumnName("is_array");

            builder.HasOne(a => a.PrimitiveDefinition).WithMany(a => a.AttributeDefinitions)
                .HasForeignKey(a => a.PrimitiveDefinitionId);
        }
    }
}