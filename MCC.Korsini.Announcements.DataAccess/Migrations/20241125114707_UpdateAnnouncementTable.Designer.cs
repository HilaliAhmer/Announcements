﻿// <auto-generated />
using System;
using MCC.Korsini.Announcements.DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    [DbContext(typeof(NotificationCenter_Context))]
    [Migration("20241125114707_UpdateAnnouncementTable")]
    partial class UpdateAnnouncementTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcement_Files_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AnnouncementId")
                        .HasColumnType("int");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AnnouncementId");

                    b.ToTable("NotificationCenter_Announcement_Files_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcement_Type_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Announcement_Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_Announcement_Type_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcements_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AnnouncementId")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("AnnouncementYear")
                        .HasColumnType("int");

                    b.Property<string>("Conten_TR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content_EN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsVisibleToAll")
                        .HasColumnType("bit");

                    b.Property<bool>("Publishing")
                        .HasColumnType("bit");

                    b.Property<string>("Title_EN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_TR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_Announcements_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Procedures_Files_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProcedureID")
                        .HasColumnType("int");

                    b.Property<int>("UploadedByUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("ProcedureID");

                    b.ToTable("NotificationCenter_Procedures_Files_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Procedures_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_Procedures_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_ScheduledAnnouncements_Files_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ScheduledAnnouncementId")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ScheduledAnnouncementId");

                    b.ToTable("NotificationCenter_ScheduledAnnouncements_Files_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_ScheduledAnnouncements_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Conten_TR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content_EN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedByUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("NextRunTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("ScheduleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScheduleTypeShow")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title_EN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title_TR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_ScheduledAnnouncements_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_UserGuides_Files_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GUID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UploadedByUserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("UploadedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserGuideID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("UserGuideID");

                    b.ToTable("NotificationCenter_UserGuides_Files_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_UserGuides_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_UserGuides_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Users_Table", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NotificationCenter_Users_Table");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcement_Files_Table", b =>
                {
                    b.HasOne("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcements_Table", "Announcement")
                        .WithMany("Files")
                        .HasForeignKey("AnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Announcement");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Procedures_Files_Table", b =>
                {
                    b.HasOne("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Procedures_Table", "Procedure")
                        .WithMany("Files")
                        .HasForeignKey("ProcedureID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Procedure");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_ScheduledAnnouncements_Files_Table", b =>
                {
                    b.HasOne("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_ScheduledAnnouncements_Table", "ScheduledAnnouncements")
                        .WithMany("Files")
                        .HasForeignKey("ScheduledAnnouncementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ScheduledAnnouncements");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_UserGuides_Files_Table", b =>
                {
                    b.HasOne("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_UserGuides_Table", "UserGuide")
                        .WithMany("Files")
                        .HasForeignKey("UserGuideID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserGuide");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Announcements_Table", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_Procedures_Table", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_ScheduledAnnouncements_Table", b =>
                {
                    b.Navigation("Files");
                });

            modelBuilder.Entity("MCC.Korsini.Announcements.Entities.Concrete.NotificationCenter_UserGuides_Table", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}
