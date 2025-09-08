using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SguScheduleAPNs.DevicesManager.Migrations
{
    /// <inheritdoc />
    public partial class ReplacedTypeWithSystemVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Devices",
                newName: "SystemVersion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SystemVersion",
                table: "Devices",
                newName: "Type");
        }
    }
}
