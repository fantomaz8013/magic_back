using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCharacterInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"),
                columns: new[] { "description", "name" },
                values: new object[] { "Ардан Громовержец — бесстрашный воин, владеющий молотом, вызывающим гром. Его мощь и отвага легендарны, а враги трепещут перед его яростью в бою.", "Ардан Громовержец" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"),
                columns: new[] { "character_race_id", "description", "name" },
                values: new object[] { 1, "Элриан Заклинатель — могущественный волшебник, повелевающий стихиями. Его знания древних магий и заклинаний делают его непревзойденным в борьбе с темными силами.", "Элриан Заклинатель" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("ce12d784-19c8-4f07-be2c-06e0c853a30e"),
                columns: new[] { "description", "name" },
                values: new object[] { "Каэл Светоносный — мудрый жрец, исцеляющий раны и изгоняющий тьму. Его сила исходит от древних богов, а сердце наполнено милосердием.", "Каэл Светоносный" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"),
                columns: new[] { "character_race_id", "description", "name" },
                values: new object[] { 2, "Тарен Лесной Страж — искусный охотник, владеющий луком и кинжалом. Он незаметно передвигается по лесу и всегда попадает в цель.", "Тарен Лесной Страж" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("1850beb4-ed84-4c7f-9a5c-cd7bce35e5d4"),
                columns: new[] { "description", "name" },
                values: new object[] { "A real man with a real COCK", "Conductor Gennady" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("40078ee4-bfce-4ee3-b54b-ff6974e4bb69"),
                columns: new[] { "character_race_id", "description", "name" },
                values: new object[] { 4, "Background: Outlander\r\nMotivation: From a young age, you couldn't abide the stink of the cities and preferred to spend your time in nature.\r\nOrigin: You grew up listening to tales of great wizards and knew you wanted to follow their path. You strove to be accepted at an academy of magic and succeeded.\r\nEvents: 2\r\nYou saw a demon and ran away before it could do anything to you.\r\n\r\nYou were accused of Assault. You were caught and convicted. You spent time in jail, chained to an oar, or performing hard labor. You served a sentence of 2 years or succeeded in escaping after that much time.", "Snugug" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("ce12d784-19c8-4f07-be2c-06e0c853a30e"),
                columns: new[] { "description", "name" },
                values: new object[] { "She had a baby but still remains virgin. Never had sex, but she is your mother. She is definitely not a whore", "Isabella The Lust" });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "character_template",
                keyColumn: "id",
                keyValue: new Guid("dfc2813c-96c3-497e-8799-ad3aa9de0ae2"),
                columns: new[] { "character_race_id", "description", "name" },
                values: new object[] { 3, "Background: Criminal\r\nMotivation: You left home and found a place in a thieves' guild or some other criminal organization.\r\nOrigin: You always had a way with animals, able to calm them with a soothing word and a touch.\r\nEvents: 1\r\nYou fought in a battle. You escaped the battle unscathed, though many of your friends were injured or lost.", "Nolgroug Berylguard" });
        }
    }
}
