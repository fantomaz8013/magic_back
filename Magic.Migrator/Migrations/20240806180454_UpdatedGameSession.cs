using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedGameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartDt",
                schema: "public",
                table: "game_session",
                newName: "PlannedStartDate");

            migrationBuilder.AddColumn<int>(
                name: "game_session_status",
                schema: "public",
                table: "game_session",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "game_session_status",
                schema: "public",
                table: "game_session");

            migrationBuilder.RenameColumn(
                name: "PlannedStartDate",
                schema: "public",
                table: "game_session",
                newName: "StartDt");
        }
    }
}
