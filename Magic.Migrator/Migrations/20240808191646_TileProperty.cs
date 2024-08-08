using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class TileProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tile_property",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    image = table.Column<string>(type: "text", nullable: false),
                    collision_type = table.Column<int>(type: "integer", nullable: false),
                    penalty_type = table.Column<int>(type: "integer", nullable: false),
                    target_type = table.Column<int>(type: "integer", nullable: false),
                    Health = table.Column<int>(type: "integer", nullable: true),
                    TilePropertyIdIfDestroyed = table.Column<int>(type: "integer", nullable: true),
                    penalty_value = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tile_property", x => x.id);
                    table.ForeignKey(
                        name: "FK_tile_property_tile_property_TilePropertyIdIfDestroyed",
                        column: x => x.TilePropertyIdIfDestroyed,
                        principalSchema: "public",
                        principalTable: "tile_property",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "tile_property",
                columns: new[] { "id", "collision_type", "description", "Health", "image", "penalty_type", "penalty_value", "target_type", "TilePropertyIdIfDestroyed", "title" },
                values: new object[,]
                {
                    { 1, 1, "Самая обычная земля", null, "storage/tiles/1.jpg", 1, null, 1, null, "Земля" },
                    { 3, 1, "Горит блять! Не ходи сцука сюда!", null, "storage/tiles/3.png", 2, 5, 1, null, "Огонь" },
                    { 4, 1, "Тут не горит, но ходить сложно", null, "storage/tiles/3.png", 3, 1, 1, null, "Вода" },
                    { 2, 3, "Самая обычная каменая стена. Невозможно пройти, но возможно сломать.", 5, "storage/tiles/2.png", 1, null, 2, 1, "Каменая стена" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tile_property_TilePropertyIdIfDestroyed",
                schema: "public",
                table: "tile_property",
                column: "TilePropertyIdIfDestroyed");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tile_property",
                schema: "public");
        }
    }
}
