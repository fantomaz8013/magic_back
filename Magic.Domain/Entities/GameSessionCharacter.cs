namespace Magic.Domain.Entities;

public class GameSessionCharacter : BaseEntity<Guid>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string AvatarUrL { get; set; }
    public int CharacterClassId { get; set; }
    public CharacterClass CharacterClass { get; set; }
    public int[] AbilitieIds { get; set; }
    public int Armor { get; set; }
    public int CharacterRaceId { get; set; }
    public CharacterRace CharacterRace { get; set; }
    public int MaxHP { get; set; }
    public int Speed { get; set; }
    public int Initiative { get; set; }

    /// <summary>
    /// Based on CharacterCharacteristic consts
    /// for example
    /// Characteristics = [10, 10, 2, 20, 15, 10]
    /// Characteristics[0] = STRENGTH VALUE
    /// Characteristics[1] = AGILITY VALUE
    /// and so on
    /// </summary>
    public Dictionary<int, int> Characteristics { get; set; }


       
    public int CurrentHP { get; set; }
    public int? CurrentShield { get; set; }
    public int? PositionX { get; set; }
    public int? PositionY { get; set; }
    public Guid OwnerId { get; set; }
    public User Owner { get; set; }
    public Guid GameSessionId { get; set; }
    public GameSession GameSession { get; set; }
}