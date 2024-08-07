using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class GameSessionCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_session_character",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    avatar_ur_l = table.Column<string>(type: "text", nullable: false),
                    character_class_id = table.Column<int>(type: "integer", nullable: false),
                    abilitie_ids = table.Column<int[]>(type: "integer[]", nullable: false),
                    armor = table.Column<int>(type: "integer", nullable: false),
                    character_race_id = table.Column<int>(type: "integer", nullable: false),
                    max_h_p = table.Column<int>(type: "integer", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    initiative = table.Column<int>(type: "integer", nullable: false),
                    characteristics = table.Column<string>(type: "text", nullable: false),
                    current_h_p = table.Column<int>(type: "integer", nullable: false),
                    current_shield = table.Column<int>(type: "integer", nullable: true),
                    position_x = table.Column<int>(type: "integer", nullable: true),
                    position_y = table.Column<int>(type: "integer", nullable: true),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: true),
                    game_session_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session_character", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_session_character_character_class_character_class_id",
                        column: x => x.character_class_id,
                        principalSchema: "public",
                        principalTable: "character_class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_session_character_character_race_character_race_id",
                        column: x => x.character_race_id,
                        principalSchema: "public",
                        principalTable: "character_race",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_session_character_game_session_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "public",
                        principalTable: "game_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_session_character_user_owner_id",
                        column: x => x.owner_id,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_character_class_id",
                schema: "public",
                table: "game_session_character",
                column: "character_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_character_race_id",
                schema: "public",
                table: "game_session_character",
                column: "character_race_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_owner_id",
                schema: "public",
                table: "game_session_character",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_session_character",
                schema: "public");
        }
    }
}
