using GalaxyMerge.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GalaxyMerge.Data.Configurations
{
    public class GObjectConfiguration : IEntityTypeConfiguration<GObject>
    {
        public void Configure(EntityTypeBuilder<GObject> builder)
        {
            //Configuring as view because EF core will treat this table as read-only, which we want.
            builder.ToView("gobject").HasKey(x => x.ObjectId);

            builder.Property(x => x.ObjectId).HasColumnName("gobject_id");
            builder.Property(x => x.TemplateId).HasColumnName("template_definition_id");
            builder.Property(x => x.DerivedFromId).HasColumnName("derived_from_gobject_id");
            builder.Property(x => x.ContainedById).HasColumnName("contained_by_gobject_id");
            builder.Property(x => x.AreaId).HasColumnName("area_gobject_id");
            builder.Property(x => x.HostId).HasColumnName("hosted_by_gobject_id");
            builder.Property(x => x.CheckedInPackageId).HasColumnName("checked_in_package_id");
            builder.Property(x => x.CheckedOutPackageId).HasColumnName("checked_out_package_id");
            builder.Property(x => x.DeployedPackageId).HasColumnName("deployed_package_id");
            builder.Property(x => x.LastDeployedPackageId).HasColumnName("last_deployed_package_id");
            builder.Property(x => x.TagName).HasColumnName("tag_name");
            builder.Property(x => x.ContainedName).HasColumnName("contained_name");
            builder.Property(x => x.HierarchicalName).HasColumnName("hierarchical_name");
            builder.Property(x => x.ConfigVersion).HasColumnName("configuration_version");
            builder.Property(x => x.DeployedVersion).HasColumnName("deployed_version");
            builder.Property(x => x.CheckedOutByUserGuid).HasColumnName("checked_out_by_user_guid");
            builder.Property(x => x.IsTemplate).HasColumnName("is_template");
            builder.Property(x => x.IsHidden).HasColumnName("is_hidden");
            builder.Property(x => x.HostingTreeLevel).HasColumnName("hosting_tree_level");
            builder.Property(x => x.DeploymentPending).HasColumnName("deployment_pending_status");

            builder.Ignore(x => x.Folder);

            builder.HasOne(x => x.TemplateDefinition).WithMany(t => t.Derivations).HasForeignKey(x => x.TemplateId);
            builder.HasOne(x => x.DerivedFrom).WithMany(t => t.Derivations).HasForeignKey(x => x.DerivedFromId);
            builder.HasOne(x => x.Container).WithMany(t => t.ContainedObjects).HasForeignKey(x => x.ContainedById);
            builder.HasOne(x => x.Area).WithMany(t => t.AreaObjects).HasForeignKey(x => x.AreaId);
            builder.HasOne(x => x.Host).WithMany(t => t.HostedObjects).HasForeignKey(x => x.HostId);
            builder.HasMany(x => x.ChangeLogs).WithOne(c => c.GObject).HasForeignKey(x => x.ObjectId);
            builder.HasOne(x => x.FolderObjectLink).WithOne(x => x.GObject).HasForeignKey<FolderObjectLink>(x => x.ObjectId);
        }
    }
}