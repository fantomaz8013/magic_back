using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class GameSessionCharacterTurnInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_session_character_turn_info",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    left_step = table.Column<int>(type: "integer", nullable: false),
                    left_main_action = table.Column<int>(type: "integer", nullable: false),
                    left_bonus_action = table.Column<int>(type: "integer", nullable: false),
                    game_session_character_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ability_cool_downs = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session_character_turn_info", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_session_character_turn_info_game_session_character_gam~",
                        column: x => x.game_session_character_id,
                        principalSchema: "public",
                        principalTable: "game_session_character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_turn_info_game_session_character_id",
                schema: "public",
                table: "game_session_character_turn_info",
                column: "game_session_character_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_session_character_turn_info",
                schema: "public");
        }
    }
}
