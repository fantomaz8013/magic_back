using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CharacterAbility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_character_class_character_characteristic_character_characte~",
                schema: "public",
                table: "character_class");

            migrationBuilder.CreateTable(
                name: "character_ability",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    distance = table.Column<int>(type: "integer", nullable: true),
                    radius = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: false),
                    targettype = table.Column<int>(name: "target_type", type: "integer", nullable: false),
                    cubetype = table.Column<int>(name: "cube_type", type: "integer", nullable: true),
                    actiontype = table.Column<int>(name: "action_type", type: "integer", nullable: false),
                    cooldowntype = table.Column<int>(name: "cool_down_type", type: "integer", nullable: false),
                    cooldowncount = table.Column<int>(name: "cool_down_count", type: "integer", nullable: true),
                    cubecount = table.Column<int>(name: "cube_count", type: "integer", nullable: true),
                    characterclassid = table.Column<int>(name: "character_class_id", type: "integer", nullable: true),
                    castercharactercharacteristicid = table.Column<int>(name: "caster_character_characteristic_id", type: "integer", nullable: true),
                    targetcharactercharacteristicid = table.Column<int>(name: "target_character_characteristic_id", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_ability", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_ability_character_characteristic_caster_character~",
                        column: x => x.castercharactercharacteristicid,
                        principalSchema: "public",
                        principalTable: "character_characteristic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_ability_character_characteristic_target_character~",
                        column: x => x.targetcharactercharacteristicid,
                        principalSchema: "public",
                        principalTable: "character_characteristic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_character_ability_character_class_character_class_id",
                        column: x => x.characterclassid,
                        principalSchema: "public",
                        principalTable: "character_class",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "character_ability",
                columns: new[] { "id", "action_type", "cool_down_type", "cube_type", "target_type", "type", "caster_character_characteristic_id", "character_class_id", "cool_down_count", "cube_count", "description", "distance", "radius", "target_character_characteristic_id", "title" },
                values: new object[,]
                {
                    { 1, 1, 3, 4, 1, 1, null, null, null, 1, "Вы наносите урон основным оружием по выбраной цели нанося 1к10 урона", 2, null, null, "Удар основным оружием" },
                    { 2, 2, 2, 4, 3, 3, null, 1, null, 1, "Вы обладаете ограниченным источником выносливости, которым можете воспользоваться, чтобы уберечь себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10", null, null, null, "Второе дыхание" },
                    { 3, 2, 2, null, 3, 4, null, 1, null, null, "Немедленно получите ещё одно действие в этом ходу. На следующий ход эффект порыва исчезает", null, null, null, "Порыв к действию" },
                    { 4, 1, 2, 4, 4, 1, null, 1, null, 1, "Удар основным оружием по конусу 3 клетки перед собой. Наносит 1к10 всем, кто находится в конусе", 1, 3, null, "Размашистый удар" },
                    { 5, 2, 4, 2, 3, 4, null, 1, null, 2, "Немедленно получите еще 3 очка действия в этом ходу, но получите 2к6 урона по себе", null, null, null, "Ярость" },
                    { 6, 1, 4, 4, 2, 1, null, 2, null, 3, "Немедленно выпускает огненый шар в точку и происходит взрыв с радиюусов 1м. Дальность 30м. Все существа в радиусе взрыва получают 3к10 урона", 30, 1, null, "Огненый шар" },
                    { 7, 1, 2, 6, 1, 5, null, 2, null, 1, "Вы внушаете определенный курс действий (ограниченный одной-двумя фразами) существу, видимому в пределах дистанции, способному слышать и понимать вас", 30, null, 5, "Внушение" },
                    { 8, 1, 3, 4, 1, 1, 5, 2, null, 1, "Вы кидаете сгусток огня в существо или предмет в пределах дистанции ( 30 м ). Совершите по цели дальнобойную атаку заклинанием. При попадании цель получает урон огнём 1к10.", 30, null, null, "Огненный снаряд" },
                    { 9, 1, 2, null, 3, 4, null, 2, null, null, "Выберите точку и перелетите к ней игнорируя все препятствия", 10, null, null, "Левитация" },
                    { 10, 1, 2, 3, 1, 3, null, 4, null, 1, "Существо, которого вы касаетесь, восстанавливает количество хитов, равное 1к8", 2, null, null, "Исцеление" },
                    { 11, 1, 2, 3, 1, 1, null, 4, null, 1, "Вы выпускаете сгусток светлой энергии по противнику, наносящий 1к8 урона и оглушающий его на 1 ход", 30, null, null, "Оглушающая кара" },
                    { 12, 1, 4, 6, 1, 3, null, 4, null, 1, "Вы можете воскресить павшего союзника c 1к20", 30, null, null, "Воскрешение" },
                    { 13, 1, 2, 4, 1, 2, null, 4, null, 1, "Вы накладываете на существо божественный щит, способный похлотить 1к10 урона", 30, null, null, "Божественный щит" },
                    { 14, 1, 2, 4, 2, 1, null, 3, null, 1, "Выпускает град стрел по указаной области, нанося всем существам 1к10 урона", 30, 1, null, "Залп стрел" },
                    { 15, 2, 2, 4, 3, 3, null, 3, null, 1, "Вы обладаете бинтами, которым можете воспользоваться, чтобы исцелить себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10", null, null, null, "Перевязка ран" },
                    { 16, 1, 3, 4, 1, 1, null, 3, null, 1, "Вы стреляете из лука по цели, нанося 1к10 урона", 30, null, null, "Точный выстрел" },
                    { 17, 1, 4, 4, 1, 1, null, 3, null, 5, "Вы стреляете из лука по цели особой стрелой, нанося 5к10 урона", 30, null, null, "Выстрел адамантиевой стрелой" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_ability_caster_character_characteristic_id",
                schema: "public",
                table: "character_ability",
                column: "caster_character_characteristic_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_ability_character_class_id",
                schema: "public",
                table: "character_ability",
                column: "character_class_id");

            migrationBuilder.CreateIndex(
                name: "IX_character_ability_target_character_characteristic_id",
                schema: "public",
                table: "character_ability",
                column: "target_character_characteristic_id");

            migrationBuilder.AddForeignKey(
                name: "FK_character_class_character_characteristic_character_characte~",
                schema: "public",
                table: "character_class",
                column: "character_characteristic_id",
                principalSchema: "public",
                principalTable: "character_characteristic",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_character_class_character_characteristic_character_characte~",
                schema: "public",
                table: "character_class");

            migrationBuilder.DropTable(
                name: "character_ability",
                schema: "public");

            migrationBuilder.AddForeignKey(
                name: "FK_character_class_character_characteristic_character_characte~",
                schema: "public",
                table: "character_class",
                column: "character_characteristic_id",
                principalSchema: "public",
                principalTable: "character_characteristic",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
