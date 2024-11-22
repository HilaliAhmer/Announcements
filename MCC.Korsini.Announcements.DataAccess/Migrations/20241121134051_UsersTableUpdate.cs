using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UsersTableUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "NotificationCenter_Users_Table");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationCenter_Users_Table");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "NotificationCenter_Users_Table",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "NotificationCenter_Users_Table");

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "NotificationCenter_Users_Table",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "NotificationCenter_Users_Table",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
