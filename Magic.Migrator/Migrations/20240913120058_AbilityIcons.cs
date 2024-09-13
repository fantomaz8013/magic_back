using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AbilityIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "icons",
                schema: "public",
                table: "character_ability",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 1,
                column: "icons",
                value: "storage/icons/ability/1.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 2,
                column: "icons",
                value: "storage/icons/ability/2.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 3,
                column: "icons",
                value: "storage/icons/ability/3.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 4,
                column: "icons",
                value: "storage/icons/ability/4.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 5,
                column: "icons",
                value: "storage/icons/ability/5.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 6,
                column: "icons",
                value: "storage/icons/ability/6.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 7,
                column: "icons",
                value: "storage/icons/ability/7.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 8,
                column: "icons",
                value: "storage/icons/ability/8.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 9,
                column: "icons",
                value: "storage/icons/ability/9.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 10,
                column: "icons",
                value: "storage/icons/ability/10.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 11,
                column: "icons",
                value: "storage/icons/ability/11.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 12,
                column: "icons",
                value: "storage/icons/ability/12.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 13,
                column: "icons",
                value: "storage/icons/ability/13.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 14,
                column: "icons",
                value: "storage/icons/ability/14.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 15,
                column: "icons",
                value: "storage/icons/ability/15.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 16,
                column: "icons",
                value: "storage/icons/ability/16.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 17,
                column: "icons",
                value: "storage/icons/ability/17.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "icons",
                schema: "public",
                table: "character_ability");
        }
    }
}
