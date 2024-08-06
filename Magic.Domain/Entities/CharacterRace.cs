namespace Magic.Domain.Entities;

/// <summary>
/// Внутриигровая раса персонажа
/// </summary>
public class CharacterRace : BaseEntity<int>
{
    public const int Human = 1;
    public const int Elf   = 2;
    public const int Dwarf = 3;
    public const int Orc   = 4;
    /// <summary>
    /// Название расы
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Описание расы
    /// </summary>
    public string Description {  get; set; }
}