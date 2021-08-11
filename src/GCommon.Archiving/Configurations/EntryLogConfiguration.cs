using GCommon.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GCommon.Archiving.Configurations
{
    public class EntryLogConfiguration : IEntityTypeConfiguration<EntryLog>
    {
        public void Configure(EntityTypeBuilder<EntryLog> builder)
        {
            builder.ToTable(nameof(EntryLog)).HasKey(x => new {x.EntryId, x.LogId});

            builder.HasIndex(x => x.EntryId).IsUnique();
            builder.HasIndex(x => x.LogId).IsUnique();

            builder.HasOne(x => x.Entry).WithOne(x => x.EntryLog).HasForeignKey<EntryLog>(x => x.EntryId);
            builder.HasOne(x => x.Log).WithOne(x => x.EntryLog).HasForeignKey<EntryLog>(x => x.LogId);
        }
    }
}