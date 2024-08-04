﻿// <auto-generated />
using System;
using Magic.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Magic.Migrator.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20240804150731_CharacterAbility")]
    partial class CharacterAbility
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Magic.Domain.Entities.CharacterAbility", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ActionType")
                        .HasColumnType("integer")
                        .HasColumnName("action_type");

                    b.Property<int>("CoolDownType")
                        .HasColumnType("integer")
                        .HasColumnName("cool_down_type");

                    b.Property<int?>("CubeType")
                        .HasColumnType("integer")
                        .HasColumnName("cube_type");

                    b.Property<int>("TargetType")
                        .HasColumnType("integer")
                        .HasColumnName("target_type");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<int?>("casterCharacterCharacteristicId")
                        .HasColumnType("integer")
                        .HasColumnName("caster_character_characteristic_id");

                    b.Property<int?>("characterClassId")
                        .HasColumnType("integer")
                        .HasColumnName("character_class_id");

                    b.Property<int?>("coolDownCount")
                        .HasColumnType("integer")
                        .HasColumnName("cool_down_count");

                    b.Property<int?>("cubeCount")
                        .HasColumnType("integer")
                        .HasColumnName("cube_count");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("distance")
                        .HasColumnType("integer")
                        .HasColumnName("distance");

                    b.Property<int?>("radius")
                        .HasColumnType("integer");

                    b.Property<int?>("targetCharacterCharacteristicId")
                        .HasColumnType("integer")
                        .HasColumnName("target_character_characteristic_id");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("casterCharacterCharacteristicId");

                    b.HasIndex("characterClassId");

                    b.HasIndex("targetCharacterCharacteristicId");

                    b.ToTable("character_ability", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ActionType = 1,
                            CoolDownType = 3,
                            CubeType = 4,
                            TargetType = 1,
                            Type = 1,
                            cubeCount = 1,
                            description = "Вы наносите урон основным оружием по выбраной цели нанося 1к10 урона",
                            distance = 2,
                            title = "Удар основным оружием"
                        },
                        new
                        {
                            Id = 2,
                            ActionType = 2,
                            CoolDownType = 2,
                            CubeType = 4,
                            TargetType = 3,
                            Type = 3,
                            characterClassId = 1,
                            cubeCount = 1,
                            description = "Вы обладаете ограниченным источником выносливости, которым можете воспользоваться, чтобы уберечь себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
                            title = "Второе дыхание"
                        },
                        new
                        {
                            Id = 3,
                            ActionType = 2,
                            CoolDownType = 2,
                            TargetType = 3,
                            Type = 4,
                            characterClassId = 1,
                            description = "Немедленно получите ещё одно действие в этом ходу. На следующий ход эффект порыва исчезает",
                            title = "Порыв к действию"
                        },
                        new
                        {
                            Id = 4,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 4,
                            TargetType = 4,
                            Type = 1,
                            characterClassId = 1,
                            cubeCount = 1,
                            description = "Удар основным оружием по конусу 3 клетки перед собой. Наносит 1к10 всем, кто находится в конусе",
                            distance = 1,
                            radius = 3,
                            title = "Размашистый удар"
                        },
                        new
                        {
                            Id = 5,
                            ActionType = 2,
                            CoolDownType = 4,
                            CubeType = 2,
                            TargetType = 3,
                            Type = 4,
                            characterClassId = 1,
                            cubeCount = 2,
                            description = "Немедленно получите еще 3 очка действия в этом ходу, но получите 2к6 урона по себе",
                            title = "Ярость"
                        },
                        new
                        {
                            Id = 6,
                            ActionType = 1,
                            CoolDownType = 4,
                            CubeType = 4,
                            TargetType = 2,
                            Type = 1,
                            characterClassId = 2,
                            cubeCount = 3,
                            description = "Немедленно выпускает огненый шар в точку и происходит взрыв с радиюусов 1м. Дальность 30м. Все существа в радиусе взрыва получают 3к10 урона",
                            distance = 30,
                            radius = 1,
                            title = "Огненый шар"
                        },
                        new
                        {
                            Id = 7,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 6,
                            TargetType = 1,
                            Type = 5,
                            characterClassId = 2,
                            cubeCount = 1,
                            description = "Вы внушаете определенный курс действий (ограниченный одной-двумя фразами) существу, видимому в пределах дистанции, способному слышать и понимать вас",
                            distance = 30,
                            targetCharacterCharacteristicId = 5,
                            title = "Внушение"
                        },
                        new
                        {
                            Id = 8,
                            ActionType = 1,
                            CoolDownType = 3,
                            CubeType = 4,
                            TargetType = 1,
                            Type = 1,
                            casterCharacterCharacteristicId = 5,
                            characterClassId = 2,
                            cubeCount = 1,
                            description = "Вы кидаете сгусток огня в существо или предмет в пределах дистанции ( 30 м ). Совершите по цели дальнобойную атаку заклинанием. При попадании цель получает урон огнём 1к10.",
                            distance = 30,
                            title = "Огненный снаряд"
                        },
                        new
                        {
                            Id = 9,
                            ActionType = 1,
                            CoolDownType = 2,
                            TargetType = 3,
                            Type = 4,
                            characterClassId = 2,
                            description = "Выберите точку и перелетите к ней игнорируя все препятствия",
                            distance = 10,
                            title = "Левитация"
                        },
                        new
                        {
                            Id = 10,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 3,
                            TargetType = 1,
                            Type = 3,
                            characterClassId = 4,
                            cubeCount = 1,
                            description = "Существо, которого вы касаетесь, восстанавливает количество хитов, равное 1к8",
                            distance = 2,
                            title = "Исцеление"
                        },
                        new
                        {
                            Id = 11,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 3,
                            TargetType = 1,
                            Type = 1,
                            characterClassId = 4,
                            cubeCount = 1,
                            description = "Вы выпускаете сгусток светлой энергии по противнику, наносящий 1к8 урона и оглушающий его на 1 ход",
                            distance = 30,
                            title = "Оглушающая кара"
                        },
                        new
                        {
                            Id = 12,
                            ActionType = 1,
                            CoolDownType = 4,
                            CubeType = 6,
                            TargetType = 1,
                            Type = 3,
                            characterClassId = 4,
                            cubeCount = 1,
                            description = "Вы можете воскресить павшего союзника c 1к20",
                            distance = 30,
                            title = "Воскрешение"
                        },
                        new
                        {
                            Id = 13,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 4,
                            TargetType = 1,
                            Type = 2,
                            characterClassId = 4,
                            cubeCount = 1,
                            description = "Вы накладываете на существо божественный щит, способный похлотить 1к10 урона",
                            distance = 30,
                            title = "Божественный щит"
                        },
                        new
                        {
                            Id = 14,
                            ActionType = 1,
                            CoolDownType = 2,
                            CubeType = 4,
                            TargetType = 2,
                            Type = 1,
                            characterClassId = 3,
                            cubeCount = 1,
                            description = "Выпускает град стрел по указаной области, нанося всем существам 1к10 урона",
                            distance = 30,
                            radius = 1,
                            title = "Залп стрел"
                        },
                        new
                        {
                            Id = 15,
                            ActionType = 2,
                            CoolDownType = 2,
                            CubeType = 4,
                            TargetType = 3,
                            Type = 3,
                            characterClassId = 3,
                            cubeCount = 1,
                            description = "Вы обладаете бинтами, которым можете воспользоваться, чтобы исцелить себя. В свой ход вы можете бонусным действием восстановить хиты в размере 1к10",
                            title = "Перевязка ран"
                        },
                        new
                        {
                            Id = 16,
                            ActionType = 1,
                            CoolDownType = 3,
                            CubeType = 4,
                            TargetType = 1,
                            Type = 1,
                            characterClassId = 3,
                            cubeCount = 1,
                            description = "Вы стреляете из лука по цели, нанося 1к10 урона",
                            distance = 30,
                            title = "Точный выстрел"
                        },
                        new
                        {
                            Id = 17,
                            ActionType = 1,
                            CoolDownType = 4,
                            CubeType = 4,
                            TargetType = 1,
                            Type = 1,
                            characterClassId = 3,
                            cubeCount = 5,
                            description = "Вы стреляете из лука по цели особой стрелой, нанося 5к10 урона",
                            distance = 30,
                            title = "Выстрел адамантиевой стрелой"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterAvatar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AvatarUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.HasKey("Id");

                    b.ToTable("character_avatar", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AvatarUrl = "storage/character/avatar/1.png"
                        },
                        new
                        {
                            Id = 2,
                            AvatarUrl = "storage/character/avatar/2.png"
                        },
                        new
                        {
                            Id = 3,
                            AvatarUrl = "storage/character/avatar/3.png"
                        },
                        new
                        {
                            Id = 4,
                            AvatarUrl = "storage/character/avatar/4.png"
                        },
                        new
                        {
                            Id = 5,
                            AvatarUrl = "storage/character/avatar/5.png"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterCharacteristic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("character_characteristic", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            description = "Проверки Силы могут моделировать попытки поднять, толкнуть, подтянуть или сломать что-то, попытки втиснуть своё тело в некое пространство или другие попытки применения грубой силы. Навык Атлетика отражает особую склонность к некоторым проверкам Силы.",
                            title = "Сила"
                        },
                        new
                        {
                            Id = 2,
                            description = "Проверка Ловкости может моделировать любую попытку перемещаться ловко, быстро или тихо, либо попытку не упасть с шаткой опоры. Навыки Акробатика, Ловкость рук и Скрытность отражают особую склонность к некоторым проверкам Ловкости.",
                            title = "Ловкость"
                        },
                        new
                        {
                            Id = 3,
                            description = "Проверки Телосложения совершаются не часто, и от него не зависят никакие навыки, так как выносливость, которую отражает эта характеристика, пассивна, и персонаж или чудовище не может активно её использовать. Однако проверка Телосложения может моделировать вашу попытку сделать что-то необычное.",
                            title = "Телосложение"
                        },
                        new
                        {
                            Id = 4,
                            description = "Проверки Интеллекта происходят когда вы используете логику, образование, память или дедуктивное мышление. Навыки История, Магия, Природа, Расследование и Религия отражают особую склонность к некоторым проверкам Интеллекта.",
                            title = "Интеллект"
                        },
                        new
                        {
                            Id = 5,
                            description = "Проверки Мудрости могут отражать попытки понять язык тела, понять чьи-то переживания, заметить что-то в окружающем мире или позаботиться о раненом. Навыки Восприятие, Выживание, Медицина, Проницательность и Уход за животными отражают особую склонность к некоторым проверкам Мудрости.",
                            title = "Мудрость"
                        },
                        new
                        {
                            Id = 6,
                            description = "Проверку Харизмы можно совершать при попытке повлиять на других или развлечь их, когда вы пытаетесь произвести впечатление или убедительно соврать, или если вы пытаетесь разобраться в сложной социальной ситуации. Навыки Выступление, Запугивание, Обман и Убеждение отражают особую склонность к некоторым проверкам Харизмы.",
                            title = "Харизма"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("characterCharacteristicId")
                        .HasColumnType("integer")
                        .HasColumnName("character_characteristic_id");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("characterCharacteristicId");

                    b.ToTable("character_class", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            characterCharacteristicId = 1,
                            description = "Опытный гладиатор сражается на арене и хорошо знает, как использовать свои трезубец и сеть, чтобы опрокинуть противника и обойти его, вызывая ликование публики и получая тактическое преимущество",
                            title = "Воин"
                        },
                        new
                        {
                            Id = 2,
                            characterCharacteristicId = 5,
                            description = "Волшебники — адепты высшей магии, объединяющиеся по типу своих заклинаний. Опираясь на тонкие плетения магии, пронизывающей вселенную, волшебники способны создавать заклинания взрывного огня, искрящихся молний, тонкого обмана и грубого контроля над сознанием.",
                            title = "Волшебник"
                        },
                        new
                        {
                            Id = 3,
                            characterCharacteristicId = 2,
                            description = "Вдали от суеты городов и посёлков, за изгородями, которые защищают самые далёкие фермы от ужасов дикой природы, среди плотно стоящих деревьев, беспутья лесов и на просторах необъятных равнин следопыты несут свой бесконечный дозор.",
                            title = "Следопыт"
                        },
                        new
                        {
                            Id = 4,
                            characterCharacteristicId = 4,
                            description = "Жрецы являются посредниками между миром смертных и далёкими мирами богов. Настолько же разные, насколько боги, которым они служат, жрецы воплощают работу своих божеств. В отличие от обычного проповедника, жрец наделён божественной магией.",
                            title = "Жрец"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterRace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("character_race", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            description = "В большинстве миров люди — это самая молодая из распространённых рас. Они поздно вышли на мировую сцену и живут намного меньше, чем дварфы, эльфы и драконы. Возможно, именно краткость их жизней заставляет их стремиться достигнуть как можно большего в отведённый им срок. А быть может, они хотят что-то доказать старшим расам, и поэтому создают могучие империи, основанные на завоеваниях и торговле. Что бы ни двигало ими, люди всегда были инноваторами и пионерами во всех мирах.",
                            title = "Человек"
                        },
                        new
                        {
                            Id = 2,
                            description = "Эльфы — это волшебный народ, обладающий неземным изяществом, живущий в мире, но не являющийся его частью. Они живут в местах, наполненных воздушной красотой, в глубинах древних лесов или в серебряных жилищах, увенчанных сверкающими шпилями и переливающихся волшебным светом. Там лёгкие дуновения ветра разносят обрывки тихих мелодий и нежные ароматы. Эльфы любят природу и магию, музыку и поэзию, и всё прекрасное, что есть в мире.",
                            title = "Эльф"
                        },
                        new
                        {
                            Id = 3,
                            description = "Полные древнего величия королевства и вырезанные в толще гор чертоги, удары кирок и молотков, раздающиеся в глубоких шахтах и пылающий кузнечный горн, верность клану и традициям и пылающая ненависть к гоблинам и оркам — вот вещи, объединяющие всех дварфов.",
                            title = "Дварф"
                        },
                        new
                        {
                            Id = 4,
                            description = "Орки — дикие грабители и налетчики; у них сутулая осанка, низкий лоб и свиноподобные лица с выступающими нижними клыками, напоминающими бивни.",
                            title = "Орк"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("city", "public");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Title = "Казань"
                        },
                        new
                        {
                            Id = 2,
                            Title = "Москва"
                        },
                        new
                        {
                            Id = 3,
                            Title = "Екатеринбург"
                        });
                });

            modelBuilder.Entity("Magic.Domain.Entities.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Category")
                        .HasColumnType("integer")
                        .HasColumnName("category");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<string>("Text")
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.HasKey("Id");

                    b.ToTable("log", "public");
                });

            modelBuilder.Entity("Magic.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text")
                        .HasColumnName("avatar_url");

                    b.Property<DateTime?>("BlockedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("blocked_date");

                    b.Property<int?>("CityId")
                        .HasColumnType("integer")
                        .HasColumnName("city_id");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("GameExperience")
                        .HasColumnType("text")
                        .HasColumnName("game_experience");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_blocked");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_salt");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("RefKey")
                        .HasColumnType("text")
                        .HasColumnName("ref_key");

                    b.Property<Guid?>("RefUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("ref_user_id");

                    b.HasKey("Id");

                    b.HasIndex("CityId");

                    b.HasIndex("RefUserId");

                    b.ToTable("user", "public");
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterAbility", b =>
                {
                    b.HasOne("Magic.Domain.Entities.CharacterCharacteristic", "CasterCharacterCharacteristic")
                        .WithMany()
                        .HasForeignKey("casterCharacterCharacteristicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Magic.Domain.Entities.CharacterClass", "CharacterClass")
                        .WithMany()
                        .HasForeignKey("characterClassId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Magic.Domain.Entities.CharacterCharacteristic", "TargetCharacterCharacteristic")
                        .WithMany()
                        .HasForeignKey("targetCharacterCharacteristicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CasterCharacterCharacteristic");

                    b.Navigation("CharacterClass");

                    b.Navigation("TargetCharacterCharacteristic");
                });

            modelBuilder.Entity("Magic.Domain.Entities.CharacterClass", b =>
                {
                    b.HasOne("Magic.Domain.Entities.CharacterCharacteristic", "characterCharacteristic")
                        .WithMany()
                        .HasForeignKey("characterCharacteristicId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("characterCharacteristic");
                });

            modelBuilder.Entity("Magic.Domain.Entities.User", b =>
                {
                    b.HasOne("Magic.Domain.Entities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Magic.Domain.Entities.User", "RefUser")
                        .WithMany()
                        .HasForeignKey("RefUserId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("City");

                    b.Navigation("RefUser");
                });
#pragma warning restore 612, 618
        }
    }
}
