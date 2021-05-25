using GalaxyMerge.Archive.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Archive.Configurations
{
    public class QueuedLogConfiguration : IEntityTypeConfiguration<QueuedLog>
    {
        public void Configure(EntityTypeBuilder<QueuedLog> builder)
        {
            builder.ToTable(nameof(QueuedLog)).HasKey(x => x.ChangeLogId);
        }
    }
}