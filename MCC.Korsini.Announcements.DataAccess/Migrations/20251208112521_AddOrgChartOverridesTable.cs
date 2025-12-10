using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MCC.Korsini.Announcements.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgChartOverridesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationCenter_DirectoryEmployee_OrgChartOverride_Table",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeSam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayManagerSam = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Scope = table.Column<byte>(type: "tinyint", nullable: false),
                    IsLocalTopManager = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationCenter_DirectoryEmployee_OrgChartOverride_Table", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationCenter_DirectoryEmployee_OrgChartOverride_Table");
        }
    }
}
