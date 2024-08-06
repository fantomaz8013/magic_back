using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class AddCharactersTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_template",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    avatar_ur_l = table.Column<string>(type: "text", nullable: false),
                    character_class_id = table.Column<int>(type: "integer", nullable: false),
                    abilitie_ids = table.Column<int[]>(type: "integer[]", nullable: false),
                    armor = table.Column<int>(type: "integer", nullable: false),
                    character_race_id = table.Column<int>(type: "integer", nullable: false),
                    max_h_p = table.Column<int>(type: "integer", nullable: false),
                    speed = table.Column<int>(type: "integer", nullable: false),
                    initiative = table.Column<int>(type: "integer", nullable: false),
                    characteristics = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_template", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_template_character_class_character_class_id",
                        column: x => x.character_class_id,
                        principalSchema: "public",
                        principalTable: "character_class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_template_character_race_character_race_id",
                        column: x => x.character_race_id,
                        principalSchema: "public",
                        principalTable: "character_race",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 13,
                column: "description",
                value: "Вы накладываете на существо божественный щит, способный поглотить 1к10 урона");

            migrationBuilder.InsertData(
                schema: "public",
                table: "character_template",
                columns: new[] { "id", "abilitie_ids", "armor", "avatar_ur_l", "character_class_id", "character_race_id", "characteristics", "description", "initiative", "max_h_p", "name", "speed" },
                values: new object[,]
                {
                    { new Guid("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"), new[] { 1, 2, 3, 4, 5 }, 12, "https://cumm.co.uk/wp-content/uploads/2023/08/00-a-mans-cock.jpg?v=1698757203", 1, 1, "{\"2\":10,\"6\":20,\"4\":5,\"5\":5,\"3\":20,\"1\":20}", "A real man with a real COCK", 5, 20, "Conductor Gennady", 8 },
                    { new Guid("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"), new[] { 1, 6, 7, 8, 9 }, 6, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ5DIPHOFPGgT1oS9B78rTLUv9RkVsWKnEccg&s", 2, 4, "{\"2\":8,\"6\":10,\"4\":20,\"5\":18,\"3\":12,\"1\":8}", "Background: Outlander\r\nMotivation: From a young age, you couldn't abide the stink of the cities and preferred to spend your time in nature.\r\nOrigin: You grew up listening to tales of great wizards and knew you wanted to follow their path. You strove to be accepted at an academy of magic and succeeded.\r\nEvents: 2\r\nYou saw a demon and ran away before it could do anything to you.\r\n\r\nYou were accused of Assault. You were caught and convicted. You spent time in jail, chained to an oar, or performing hard labor. You served a sentence of 2 years or succeeded in escaping after that much time.", 4, 14, "Snugug", 8 },
                    { new Guid("ce12d784-19c8-4f07-be2c-06e0c853a30e"), new[] { 1, 10, 11, 12, 13 }, 8, "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg", 4, 2, "{\"2\":8,\"6\":20,\"4\":12,\"5\":15,\"3\":14,\"1\":5}", "She had a baby but still remains virgin. Never had sex, but she is your mother. She is definitely not a whore", 3, 16, "Isabella The Lust", 8 },
                    { new Guid("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"), new[] { 1, 14, 15, 16, 17 }, 10, "https://source.boomplaymusic.com/group10/M00/02/07/40af9aa9b99e46aa8f205d25fe687fa9_320_320.jpg", 3, 3, "{\"2\":16,\"6\":10,\"4\":10,\"5\":10,\"3\":16,\"1\":10}", "Background: Criminal\r\nMotivation: You left home and found a place in a thieves' guild or some other criminal organization.\r\nOrigin: You always had a way with animals, able to calm them with a soothing word and a touch.\r\nEvents: 1\r\nYou fought in a battle. You escaped the battle unscathed, though many of your friends were injured or lost.", 7, 18, "Nolgroug Berylguard", 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_template_character_class_id",
                schema: "public",
                table: "character_template",
                column: "character_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_template_character_race_id",
                schema: "public",
                table: "character_template",
                column: "character_race_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_template",
                schema: "public");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_ability",
                keyColumn: "id",
                keyValue: 13,
                column: "description",
                value: "Вы накладываете на существо божественный щит, способный похлотить 1к10 урона");
        }
    }
}
