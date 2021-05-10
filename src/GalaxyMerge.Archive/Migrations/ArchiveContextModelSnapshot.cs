﻿// <auto-generated />
using System;
using GalaxyMerge.Archive;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GalaxyMerge.Archive.Migrations
{
    [DbContext(typeof(ArchiveContext))]
    partial class ArchiveContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.ArchiveEntry", b =>
                {
                    b.Property<Guid>("EntryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ArchivedOn")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("CompressedData")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<long>("CompressedSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ObjectId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("OriginalSize")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("EntryId");

                    b.HasIndex("ObjectId");

                    b.ToTable("ArchiveEntry");
                });

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.ArchiveObject", b =>
                {
                    b.Property<int>("ObjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AddedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Version")
                        .HasColumnType("INTEGER");

                    b.HasKey("ObjectId");

                    b.ToTable("ArchiveObject");
                });

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.EventSetting", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsArchiveTrigger")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OperationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OperationType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.HasIndex("OperationId")
                        .IsUnique();

                    b.HasIndex("OperationName")
                        .IsUnique();

                    b.ToTable("EventSetting");
                });

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.GalaxyInfo", b =>
                {
                    b.Property<string>("GalaxyName")
                        .HasColumnType("TEXT");

                    b.Property<string>("CdiVersion")
                        .HasColumnType("TEXT");

                    b.Property<string>("IsaVersion")
                        .HasColumnType("TEXT");

                    b.Property<int?>("VersionNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("GalaxyName");

                    b.ToTable("GalaxyInfo");
                });

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.InclusionSetting", b =>
                {
                    b.Property<int>("InclusionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IncludesInstances")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InclusionOption")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TemplateId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TemplateName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("InclusionId");

                    b.HasIndex("TemplateId")
                        .IsUnique();

                    b.HasIndex("TemplateName")
                        .IsUnique();

                    b.ToTable("InclusionSetting");
                });

            modelBuilder.Entity("GalaxyMerge.Archive.Entities.ArchiveEntry", b =>
                {
                    b.HasOne("GalaxyMerge.Archive.Entities.ArchiveObject", "ArchiveObject")
                        .WithMany("Entries")
                        .HasForeignKey("ObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
