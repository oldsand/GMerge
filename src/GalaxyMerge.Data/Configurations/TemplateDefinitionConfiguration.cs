using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class TemplateDefinitionConfiguration : IEntityTypeConfiguration<TemplateDefinition>
    {
        public void Configure(EntityTypeBuilder<TemplateDefinition> builder)
        {
            //Configuring as view because EF core will treat this table as read-only, which we want.
            builder.ToView("template_definition").HasKey(t => t.TemplateDefinitionId);

            builder.Property(t => t.TemplateDefinitionId).HasColumnName("template_definition_id");
            builder.Property(t => t.ObjectId).HasColumnName("base_gobject_id");
            builder.Property(t => t.TagName).HasColumnName("original_template_tagname");
            builder.Property(t => t.CategoryId).HasColumnName("category_id");
            builder.Property(t => t.Codebase).HasColumnName("codebase");
        }
    }
}