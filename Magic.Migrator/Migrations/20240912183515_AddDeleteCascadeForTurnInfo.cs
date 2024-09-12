using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddDeleteCascadeForTurnInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_turn_info_game_session_character_gam~",
                schema: "public",
                table: "game_session_character_turn_info");

            migrationBuilder.AddForeignKey(
                name: "FK_game_session_character_turn_info_game_session_character_gam~",
                schema: "public",
                table: "game_session_character_turn_info",
                column: "game_session_character_id",
                principalSchema: "public",
                principalTable: "game_session_character",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_turn_info_game_session_character_gam~",
                schema: "public",
                table: "game_session_character_turn_info");

            migrationBuilder.AddForeignKey(
                name: "FK_game_session_character_turn_info_game_session_character_gam~",
                schema: "public",
                table: "game_session_character_turn_info",
                column: "game_session_character_id",
                principalSchema: "public",
                principalTable: "game_session_character",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
