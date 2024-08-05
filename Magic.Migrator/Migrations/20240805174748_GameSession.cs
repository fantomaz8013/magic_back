using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class GameSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "game_session",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    maxusercount = table.Column<int>(name: "max_user_count", type: "integer", nullable: false),
                    CreatorUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    createddate = table.Column<DateTime>(name: "created_date", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session", x => x.id);
                    table.ForeignKey(
                        name: "FK_game_session_user_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameSessionUser",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameSessionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameSessionUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameSessionUser_game_session_GameSessionId",
                        column: x => x.GameSessionId,
                        principalSchema: "public",
                        principalTable: "game_session",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameSessionUser_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_game_session_CreatorUserId",
                schema: "public",
                table: "game_session",
                column: "CreatorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessionUser_GameSessionId",
                schema: "public",
                table: "GameSessionUser",
                column: "GameSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GameSessionUser_UserId",
                schema: "public",
                table: "GameSessionUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameSessionUser",
                schema: "public");

            migrationBuilder.DropTable(
                name: "game_session",
                schema: "public");
        }
    }
}
