using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersColumng : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                schema: "public",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "city_id",
                schema: "public",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                schema: "public",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "game_experience",
                schema: "public",
                table: "user",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "city",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_city", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "city",
                columns: new[] { "id", "title" },
                values: new object[,]
                {
                    { 1, "Казань" },
                    { 2, "Москва" },
                    { 3, "Екатеринбург" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_city_id",
                schema: "public",
                table: "user",
                column: "city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_user_city_city_id",
                schema: "public",
                table: "user",
                column: "city_id",
                principalSchema: "public",
                principalTable: "city",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_city_city_id",
                schema: "public",
                table: "user");

            migrationBuilder.DropTable(
                name: "city",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_user_city_id",
                schema: "public",
                table: "user");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                schema: "public",
                table: "user");

            migrationBuilder.DropColumn(
                name: "city_id",
                schema: "public",
                table: "user");

            migrationBuilder.DropColumn(
                name: "description",
                schema: "public",
                table: "user");

            migrationBuilder.DropColumn(
                name: "game_experience",
                schema: "public",
                table: "user");
        }
    }
}
