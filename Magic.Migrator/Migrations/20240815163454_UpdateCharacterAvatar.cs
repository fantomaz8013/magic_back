using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacterAvatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"),
                column: "avatar_ur_l",
                value: "storage/character/avatar/1.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"),
                column: "avatar_ur_l",
                value: "storage/character/avatar/4.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("ce12d784-19c8-4f07-be2c-06e0c853a30e"),
                column: "avatar_ur_l",
                value: "storage/character/avatar/2.png");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"),
                column: "avatar_ur_l",
                value: "storage/character/avatar/3.png");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"),
                column: "avatar_ur_l",
                value: "https://cumm.co.uk/wp-content/uploads/2023/08/00-a-mans-cock.jpg?v=1698757203");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"),
                column: "avatar_ur_l",
                value: "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5DIPHOFPGgT1oS9B78rTLUv9RkVsWKnEccg&s");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("ce12d784-19c8-4f07-be2c-06e0c853a30e"),
                column: "avatar_ur_l",
                value: "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"),
                column: "avatar_ur_l",
                value: "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg");
        }
    }
}
