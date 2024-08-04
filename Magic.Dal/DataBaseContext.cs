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
    public DbSet<CharacterAvatar> CharacterAvatars { get; set; }
    public DbSet<CharacterCharacteristic> CharacterCharacteristic { get; set; }
    public DbSet<CharacterClass> CharacterClasses { get; set; }
    public DbSet<CharacterRace> CharacterRaces { get; set; }
    public DbSet<CharacterAbility > CharacterAbilities { get; set; }

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
        modelBuilder.ApplyConfiguration(new CharacterAbilityConfiguration());
    }
}