using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CharacterBuff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "buff_cool_downs",
                schema: "public",
                table: "game_session_character_turn_info",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "skip_step_count",
                schema: "public",
                table: "game_session_character_turn_info",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuffCount",
                schema: "public",
                table: "character_ability",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "character_buff_id",
                schema: "public",
                table: "character_ability",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "game_session_character_buff",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    is_negative = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    buff_type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_game_session_character_buff", x => x.id);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { 1, 3 });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { 3, 3 });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { 1, 4 });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 17,
                columns: new[] { "BuffCount", "character_buff_id" },
                values: new object[] { null, null });

            migrationBuilder.InsertData(
                schema: "public",
                table: "game_session_character_buff",
                columns: new[] { "id", "buff_type", "description", "is_negative", "title" },
                values: new object[] { 1, 1, "Персонаж нейтрализован. Он не может совершать действия и пропускает ход", true, "Оглушение" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "game_session_character_buff",
                columns: new[] { "id", "buff_type", "description", "title" },
                values: new object[,]
                {
                    { 2, 2, "Персонаж научился летать, теперь он может перемещаться по блокам доступных для полета", "Полет" },
                    { 3, 3, "Персонаж получил дополнительные очки действия", "Дополнительное действие" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "game_session_character_buff",
                columns: new[] { "id", "buff_type", "description", "is_negative", "title" },
                values: new object[] { 4, 4, "Персонаж внушен и должен выполнить заданные ему дейсвтия", true, "Внушение" });

            migrationBuilder.CreateIndex(
                name: "IX_character_ability_character_buff_id",
                schema: "public",
                table: "character_ability",
                column: "character_buff_id");

            migrationBuilder.AddForeignKey(
                name: "FK_character_ability_game_session_character_buff_character_buf~",
                schema: "public",
                table: "character_ability",
                column: "character_buff_id",
                principalSchema: "public",
                principalTable: "game_session_character_buff",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_character_ability_game_session_character_buff_character_buf~",
                schema: "public",
                table: "character_ability");

            migrationBuilder.DropTable(
                name: "game_session_character_buff",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_character_ability_character_buff_id",
                schema: "public",
                table: "character_ability");

            migrationBuilder.DropColumn(
                name: "buff_cool_downs",
                schema: "public",
                table: "game_session_character_turn_info");

            migrationBuilder.DropColumn(
                name: "skip_step_count",
                schema: "public",
                table: "game_session_character_turn_info");

            migrationBuilder.DropColumn(
                name: "BuffCount",
                schema: "public",
                table: "character_ability");

            migrationBuilder.DropColumn(
                name: "character_buff_id",
                schema: "public",
                table: "character_ability");
        }
    }
}
