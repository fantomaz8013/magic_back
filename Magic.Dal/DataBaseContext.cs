using Magic.DAL.Configurations;
using Magic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Magic.DAL;

public class DataBaseContext : DbContext
{
    private const string PUBLIC = "public";

    public DbSet<Log> Log { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<CharacterTemplate> CharacterTemplates { get; set; }
    public DbSet<CharacterAvatar> CharacterAvatars { get; set; }
    public DbSet<CharacterCharacteristic> CharacterCharacteristic { get; set; }
    public DbSet<CharacterClass> CharacterClasses { get; set; }
    public DbSet<CharacterRace> CharacterRaces { get; set; }
    public DbSet<CharacterAbility > CharacterAbilities { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<GameSessionUser> GameSessionUser { get; set; }
    public DbSet<BaseGameSessionMessage> GameSessionMessages { get; set; }
    public DbSet<ChatGameGameSessionMessage> ChatGameSessionMessages { get; set; }
    public DbSet<ServerGameSessionMessage> ServerSessionMessages { get; set; }
    public DbSet<DiceGameSessionMessage> DiceSessionMessages { get; set; }
    public DbSet<GameSessionCharacter> GameSessionCharacters { get; set; }
    public DbSet<Map> Maps { get; set; }
    public DbSet<TileProperty> TileProperties { get; set; }
    public DbSet<GameSessionCharacterTurnInfo> GameSessionCharacterTurnInfos { get; set; }

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(PUBLIC);

        modelBuilder.ApplyConfiguration(new LogConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CityConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterAvatarConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterCharacteristicConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterClassConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterRaceConfiguration());
        modelBuilder.ApplyConfiguration(new GameSessionCharacterBuffConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterAbilityConfiguration());
        modelBuilder.ApplyConfiguration(new GameSessionConfiguration());
        modelBuilder.ApplyConfiguration(new BaseGameSessionMessageConfiguration());
        modelBuilder.ApplyConfiguration(new ChatGameSessionMessageConfiguration());
        modelBuilder.ApplyConfiguration(new ServerGameSessionMessageConfiguration());
        modelBuilder.ApplyConfiguration(new DiceGameSessionMessageConfiguration());
        modelBuilder.ApplyConfiguration(new CharacterTemplateConfiguration());
        modelBuilder.ApplyConfiguration(new GameSessionCharacterConfiguration());
        modelBuilder.ApplyConfiguration(new MapConfiguration());
        modelBuilder.ApplyConfiguration(new TilePropertyConfiguration());
        modelBuilder.ApplyConfiguration(new GameSessionCharacterTurnInfoConfiguration());
    }
}