using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserTableExtend : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "NotificationCenter_Users_Table",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "NotificationCenter_Users_Table",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "NotificationCenter_Users_Table",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "NotificationCenter_Users_Table");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "NotificationCenter_Users_Table");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "NotificationCenter_Users_Table");
        }
    }
}
