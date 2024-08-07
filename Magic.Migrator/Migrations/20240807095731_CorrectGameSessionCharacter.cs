using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CorrectGameSessionCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_game_session_owner_id",
                schema: "public",
                table: "game_session_character");

            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                schema: "public",
                table: "game_session_character",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_game_session_character_game_session_id",
                schema: "public",
                table: "game_session_character",
                column: "game_session_id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_game_session_character_game_session_game_session_id",
                schema: "public",
                table: "game_session_character");

            migrationBuilder.DropIndex(
                name: "IX_game_session_character_game_session_id",
                schema: "public",
                table: "game_session_character");

            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                schema: "public",
                table: "game_session_character",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_game_session_character_game_session_owner_id",
                schema: "public",
                table: "game_session_character",
                column: "owner_id",
                principalSchema: "public",
                principalTable: "game_session",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
