using GalaxyMerge.Client.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLog;

namespace GalaxyMerge.Client.Data.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.ToTable("Log").HasKey(x => x.LogId);
            builder.Ignore(x => x.LogLevel);
        }
    }
}