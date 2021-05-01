using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class ArchiveExclusionConfiguration : IEntityTypeConfiguration<ArchiveTemplate>
    {
        public void Configure(EntityTypeBuilder<ArchiveTemplate> builder)
        {
            builder.ToTable("Exclusion").HasKey(x => x.TemplateId);
            builder.Property(x => x.TemplateName).IsRequired();
        }
    }
}