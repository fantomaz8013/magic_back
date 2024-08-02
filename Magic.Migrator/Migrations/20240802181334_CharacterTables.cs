using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Magic.Migrator.Migrations
{
    /// <inheritdoc />
    public partial class CharacterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "character_characteristic",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_characteristic", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "character_class",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    charactercharacteristicid = table.Column<int>(name: "character_characteristic_id", type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_character_class", x => x.id);
                    table.ForeignKey(
                        name: "FK_character_class_character_characteristic_character_characte~",
                        column: x => x.charactercharacteristicid,
                        principalSchema: "public",
                        principalTable: "character_characteristic",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "character_characteristic",
                columns: new[] { "id", "description", "title" },
                values: new object[,]
                {
                    { 1, "Проверки Силы могут моделировать попытки поднять, толкнуть, подтянуть или сломать что-то, попытки втиснуть своё тело в некое пространство или другие попытки применения грубой силы. Навык Атлетика отражает особую склонность к некоторым проверкам Силы.", "Сила" },
                    { 2, "Проверка Ловкости может моделировать любую попытку перемещаться ловко, быстро или тихо, либо попытку не упасть с шаткой опоры. Навыки Акробатика, Ловкость рук и Скрытность отражают особую склонность к некоторым проверкам Ловкости.", "Ловкость" },
                    { 3, "Проверки Телосложения совершаются не часто, и от него не зависят никакие навыки, так как выносливость, которую отражает эта характеристика, пассивна, и персонаж или чудовище не может активно её использовать. Однако проверка Телосложения может моделировать вашу попытку сделать что-то необычное.", "Телосложение" },
                    { 4, "Проверки Интеллекта происходят когда вы используете логику, образование, память или дедуктивное мышление. Навыки История, Магия, Природа, Расследование и Религия отражают особую склонность к некоторым проверкам Интеллекта.", "Интеллект" },
                    { 5, "Проверки Мудрости могут отражать попытки понять язык тела, понять чьи-то переживания, заметить что-то в окружающем мире или позаботиться о раненом. Навыки Восприятие, Выживание, Медицина, Проницательность и Уход за животными отражают особую склонность к некоторым проверкам Мудрости.", "Мудрость" },
                    { 6, "Проверку Харизмы можно совершать при попытке повлиять на других или развлечь их, когда вы пытаетесь произвести впечатление или убедительно соврать, или если вы пытаетесь разобраться в сложной социальной ситуации. Навыки Выступление, Запугивание, Обман и Убеждение отражают особую склонность к некоторым проверкам Харизмы.", "Харизма" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "character_class",
                columns: new[] { "id", "character_characteristic_id", "description", "title" },
                values: new object[,]
                {
                    { 1, 1, "Опытный гладиатор сражается на арене и хорошо знает, как использовать свои трезубец и сеть, чтобы опрокинуть противника и обойти его, вызывая ликование публики и получая тактическое преимущество", "Воин" },
                    { 2, 5, "Волшебники — адепты высшей магии, объединяющиеся по типу своих заклинаний. Опираясь на тонкие плетения магии, пронизывающей вселенную, волшебники способны создавать заклинания взрывного огня, искрящихся молний, тонкого обмана и грубого контроля над сознанием.", "Волшебник" },
                    { 3, 2, "Вдали от суеты городов и посёлков, за изгородями, которые защищают самые далёкие фермы от ужасов дикой природы, среди плотно стоящих деревьев, беспутья лесов и на просторах необъятных равнин следопыты несут свой бесконечный дозор.", "Следопыт" },
                    { 4, 4, "Жрецы являются посредниками между миром смертных и далёкими мирами богов. Настолько же разные, насколько боги, которым они служат, жрецы воплощают работу своих божеств. В отличие от обычного проповедника, жрец наделён божественной магией.", "Жрец" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_character_class_character_characteristic_id",
                schema: "public",
                table: "character_class",
                column: "character_characteristic_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_class",
                schema: "public");

            migrationBuilder.DropTable(
                name: "character_characteristic",
                schema: "public");
        }
    }
}
