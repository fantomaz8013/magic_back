using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CharacterAvatarTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_avatar",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    avatarurl = table.Column<string>(name: "avatar_url", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_avatar", x => x.id);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "character_avatar",
                columns: new[] { "id", "avatar_url" },
                values: new object[,]
                {
                    { 1, "storage/character/avatar/1.png" },
                    { 2, "storage/character/avatar/2.png" },
                    { 3, "storage/character/avatar/3.png" },
                    { 4, "storage/character/avatar/4.png" },
                    { 5, "storage/character/avatar/5.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_avatar",
                schema: "public");
        }
    }
}
