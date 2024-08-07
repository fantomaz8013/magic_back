using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddedCascadeDeleteGameSessionCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_game_session_game_session_id",
                schema: "public",
                table: "game_session_character");

            migrationBuilder.AddForeignKey(
                name: "FK_game_session_character_game_session_game_session_id",
                schema: "public",
                table: "game_session_character",
                column: "game_session_id",
                principalSchema: "public",
                principalTable: "game_session",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_game_session_game_session_id",
                schema: "public",
                table: "game_session_character");

            migrationBuilder.AddForeignKey(
                name: "FK_game_session_character_game_session_game_session_id",
                schema: "public",
                table: "game_session_character",
                column: "game_session_id",
                principalSchema: "public",
                principalTable: "game_session",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
