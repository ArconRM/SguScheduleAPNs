using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SguScheduleAPNs.DevicesManager.Migrations
{
    /// <inheritdoc />
    public partial class AddedRegisteredAtField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RegisteredAt",
                table: "Devices",
                type: "bigint",
                nullable: false,
                defaultValueSql: "(EXTRACT(EPOCH FROM NOW()) * 1000)::bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "Devices");
        }
    }
}
