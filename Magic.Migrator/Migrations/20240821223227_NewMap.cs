using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class NewMap : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "map",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-1234-cd7bce35e5d4"),
                column: "tiles",
                value: "[[5,5,5,5,5,5,5,1,2,5,1,5,5,5,1],[5,5,5,5,5,4,5,1,5,5,1,5,5,5,1],[5,5,5,5,5,5,5,1,5,5,1,5,5,2,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[5,5,5,5,5,4,4,4,4,4,1,5,3,3,1],[5,5,5,5,5,4,4,4,4,2,1,5,3,3,1],[1,1,3,3,3,1,1,4,4,4,1,1,1,1,1],[5,5,3,3,3,5,5,1,5,5,1,5,5,5,1],[5,5,3,3,3,5,5,1,5,5,1,5,5,5,1],[5,5,5,5,5,5,5,1,5,5,1,5,5,5,2],[5,5,5,5,5,5,5,1,5,5,2,5,5,5,1],[2,5,5,5,5,5,5,1,5,5,1,2,2,5,1],[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1],[5,5,5,5,5,5,5,1,5,5,1,5,5,5,2],[5,5,5,5,5,5,5,1,5,5,1,5,5,2,1]]");

            migrationBuilder.InsertData(
                schema: "public",
                table: "tile_property",
                columns: new[] { "id", "collision_type", "description", "Health", "image", "penalty_type", "penalty_value", "target_type", "TilePropertyIdIfDestroyed", "title" },
                values: new object[] { 5, 1, "Самая обычная трава ( хих ) ", null, "storage/tiles/5.png", 1, null, 1, null, "Трава" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "tile_property",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "map",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-1234-cd7bce35e5d4"),
                column: "tiles",
                value: "[[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],[0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]]");
        }
    }
}
