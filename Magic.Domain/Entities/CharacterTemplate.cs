﻿namespace Magic.Domain.Entities;

public class CharacterTemplate : BaseEntity<Guid>
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
}

