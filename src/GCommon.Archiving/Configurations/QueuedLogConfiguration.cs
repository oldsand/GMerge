using GCommon.Archiving.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class QueuedLogConfiguration : IEntityTypeConfiguration<QueuedLog>
    {
        public void Configure(EntityTypeBuilder<QueuedLog> builder)
        {
            builder.ToTable(nameof(QueuedLog)).HasKey(x => x.ChangeLogId);
            builder.Property(x => x.QueuedOn).IsRequired();
        }
    }
}