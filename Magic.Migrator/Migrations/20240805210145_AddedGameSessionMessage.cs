using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameSessionMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_session_messages",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    gamesessionmessagetypeenum = table.Column<int>(name: "game_session_message_type_enum", type: "integer", nullable: false),
                    gamesessionid = table.Column<Guid>(name: "game_session_id", type: "uuid", nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp with time zone", nullable: false),
                    roll = table.Column<int>(type: "integer", nullable: true),
                    cubetypeenum = table.Column<int>(name: "cube_type_enum", type: "integer", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    authorid = table.Column<Guid>(name: "author_id", type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session_messages", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_session_messages_game_session_game_session_id",
                        column: x => x.gamesessionid,
                        principalSchema: "public",
                        principalTable: "game_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_game_session_messages_user_author_id",
                        column: x => x.authorid,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_session_messages_author_id",
                schema: "public",
                table: "game_session_messages",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_game_session_messages_game_session_id",
                schema: "public",
                table: "game_session_messages",
                column: "game_session_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "game_session_messages",
                schema: "public");
        }
    }
}
