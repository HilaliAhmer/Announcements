using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationCenter_Announcement_Type_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Announcement_Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Announcement_Type_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_Announcements_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title_TR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conten_TR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content_EN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    IsVisibleToAll = table.Column<bool>(type: "bit", nullable: false),
                    Publishing = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Announcements_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_Procedures_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Procedures_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_ScheduledAnnouncements_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title_TR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conten_TR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title_EN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content_EN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduleTypeShow = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextRunTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_ScheduledAnnouncements_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_UserGuides_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_UserGuides_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_Users_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Users_Table", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_Announcement_Files_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Announcement_Files_Table", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotificationCenter_Announcement_Files_Table_NotificationCenter_Announcements_Table_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "NotificationCenter_Announcements_Table",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_Procedures_Files_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcedureID = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedByUserID = table.Column<int>(type: "int", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_Procedures_Files_Table", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotificationCenter_Procedures_Files_Table_NotificationCenter_Procedures_Table_ProcedureID",
                        column: x => x.ProcedureID,
                        principalTable: "NotificationCenter_Procedures_Table",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_ScheduledAnnouncements_Files_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduledAnnouncementId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_ScheduledAnnouncements_Files_Table", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotificationCenter_ScheduledAnnouncements_Files_Table_NotificationCenter_ScheduledAnnouncements_Table_ScheduledAnnouncementId",
                        column: x => x.ScheduledAnnouncementId,
                        principalTable: "NotificationCenter_ScheduledAnnouncements_Table",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCenter_UserGuides_Files_Table",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GUID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserGuideID = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadedByUserID = table.Column<int>(type: "int", nullable: false),
                    UploadedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_UserGuides_Files_Table", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NotificationCenter_UserGuides_Files_Table_NotificationCenter_UserGuides_Table_UserGuideID",
                        column: x => x.UserGuideID,
                        principalTable: "NotificationCenter_UserGuides_Table",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCenter_Announcement_Files_Table_AnnouncementId",
                table: "NotificationCenter_Announcement_Files_Table",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCenter_Procedures_Files_Table_ProcedureID",
                table: "NotificationCenter_Procedures_Files_Table",
                column: "ProcedureID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCenter_ScheduledAnnouncements_Files_Table_ScheduledAnnouncementId",
                table: "NotificationCenter_ScheduledAnnouncements_Files_Table",
                column: "ScheduledAnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCenter_UserGuides_Files_Table_UserGuideID",
                table: "NotificationCenter_UserGuides_Files_Table",
                column: "UserGuideID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationCenter_Announcement_Files_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_Announcement_Type_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_Procedures_Files_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_ScheduledAnnouncements_Files_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_UserGuides_Files_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_Users_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_Announcements_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_Procedures_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_ScheduledAnnouncements_Table");

            migrationBuilder.DropTable(
                name: "NotificationCenter_UserGuides_Table");
        }
    }
}
