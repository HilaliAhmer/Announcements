using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDirectoryEmployeeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationCenter_DirectoryEmployee_Table",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    // AD Kimlik Bilgileri
                    SamAccountName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DistinguishedName = table.Column<string>(type: "nvarchar(max)", nullable: false),

                    // Kullanıcı Bilgileri
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),

                    // Organizasyon ilişkisi
                    ManagerId = table.Column<int>(type: "int", nullable: true),

                    // Genel
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        name: "PK_NotificationCenter_DirectoryEmployee_Table",
                        x => x.Id);

                    table.ForeignKey(
                        name: "FK_NotificationCenter_DirectoryEmployee_Table_NotificationCenter_DirectoryEmployee_Table_ManagerId",
                        column: x => x.ManagerId,
                        principalTable: "NotificationCenter_DirectoryEmployee_Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCenter_DirectoryEmployee_Table_ManagerId",
                table: "NotificationCenter_DirectoryEmployee_Table",
                column: "ManagerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationCenter_DirectoryEmployee_Table");
        }
    }
}
