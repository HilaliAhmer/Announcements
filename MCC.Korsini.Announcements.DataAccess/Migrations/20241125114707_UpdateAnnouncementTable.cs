using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnnouncementTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnouncementYear",
                table: "NotificationCenter_Announcements_Table",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnouncementYear",
                table: "NotificationCenter_Announcements_Table");
        }
    }
}
