using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class ChangeLogConfiguration : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            //Configuring as view because EF core will treat this table as read-only, which we want.
            builder.ToView("gobject_change_log").HasKey(x => x.ChangeLogId);

            builder.Property(x => x.ChangeLogId).HasColumnName("gobject_change_log_id");
            builder.Property(x => x.ObjectId).HasColumnName("gobject_id");
            builder.Property(x => x.ChangeDate).HasColumnName("change_date");
            builder.Property(x => x.OperationId).HasColumnName("operation_id");
            builder.Property(x => x.Comment).HasColumnName("user_comment");
            builder.Property(x => x.ConfigurationVersion).HasColumnName("configuration_version");
            builder.Property(x => x.UserName).HasColumnName("user_profile_name");
        }
    }
}