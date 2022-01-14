using GClient.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLog;

namespace GClient.Data.Configurations
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