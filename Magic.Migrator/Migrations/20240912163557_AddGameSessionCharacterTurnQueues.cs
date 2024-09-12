using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddGameSessionCharacterTurnQueues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_session_character_turn_queue",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    game_session_character_ids = table.Column<string>(type: "text", nullable: false),
                    current_index = table.Column<int>(type: "integer", nullable: false),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session_character_turn_queue", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_session_character_turn_queue_game_session_game_session~",
                        column: x => x.game_session_id,
                        principalSchema: "public",
                        principalTable: "game_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "tile_property",
                keyColumn: "id",
                keyValue: 4,
                column: "image",
                value: "storage/tiles/4.png");

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_turn_queue_game_session_id",
                schema: "public",
                table: "game_session_character_turn_queue",
                column: "game_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_session_character_turn_queue",
                schema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "tile_property",
                keyColumn: "id",
                keyValue: 4,
                column: "image",
                value: "storage/tiles/3.png");
        }
    }
}
