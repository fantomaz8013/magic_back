namespace Magic.Domain.Entities;

/// <summary>
/// Внутриигровая раса персонажа
/// </summary>
public class CharacterRace : BaseEntity<int>
{
    public const int HUMAN = 1;
    public const int ELF   = 2;
    public const int DWARF = 3;
    public const int ORC   = 4;
    /// <summary>
    /// Название расы
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// Описание расы
    /// </summary>
    public string description {  get; set; }
}