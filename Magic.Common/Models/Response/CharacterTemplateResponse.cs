using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class CharacterTemplateResponse
{
    public CharacterTemplateResponse(CharacterTemplate template, CharacterAbility[] abilities)
    {
        Id = template.Id;
        Name = template.Name;
        Description = template.Description;
        AvatarUrL = template.AvatarUrL;
        CharacterClass = template.CharacterClass;
        Abilities = abilities;
        Armor = template.Armor;
        CharacterRaceId = template.CharacterRaceId;
        CharacterRace = template.CharacterRace;
        MaxHP = template.MaxHP;
        Speed = template.Speed;
        Initiative = template.Initiative;
        Characteristics = template.Characteristics;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string AvatarUrL { get; set; }
    public CharacterClass CharacterClass { get; set; }
    public CharacterAbility[] Abilities { get; set; }
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
}