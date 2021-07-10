using GCommon.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Data.Configurations
{
    public class PrimitiveDefinitionConfiguration : IEntityTypeConfiguration<PrimitiveDefinition>
    {
        public void Configure(EntityTypeBuilder<PrimitiveDefinition> builder)
        {
            builder.ToView("primitive_definition").HasKey(p => p.PrimitiveDefinitionId);
            
            builder.Property(p => p.TemplateDefinitionId).HasColumnName("template_definition_id");
            builder.Property(p => p.ParentPrimitiveId).HasColumnName("parent_mx_primitive_id");
            builder.Property(p => p.PrimitiveId).HasColumnName("mx_primitive_id");
            builder.Property(p => p.PrimitiveName).HasColumnName("primitive_name");
            builder.Property(p => p.ExecutionGroup).HasColumnName("execution_group");
            builder.Property(p => p.IsVirtual).HasColumnName("is_virtual");
            builder.Property(p => p.SupportsDynamicAttributes).HasColumnName("supports_dynamic_attributes");
            builder.Property(p => p.MajorVersion).HasColumnName("major_version");

            builder.HasOne(p => p.TemplateDefinition).WithMany(p => p.PrimitiveDefinitions)
                .HasForeignKey(p => p.TemplateDefinitionId);
        }
    }
}