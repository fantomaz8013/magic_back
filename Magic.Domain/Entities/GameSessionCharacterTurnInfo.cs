using Magic.Domain.Enums;

namespace Magic.Domain.Entities;

public class GameSessionCharacterTurnInfo : BaseEntity<int>
{
    /// <summary>
    /// Количество оставшегося передвижения
    /// </summary>
    public int LeftStep { get; set; }
    /// <summary>
    /// Количество оставшихся очков действия
    /// </summary>
    public int LeftMainAction { get; set; }
    /// <summary>
    /// Количество оставшихся очков бонусного действия
    /// </summary>
    public int LeftBonusAction { get; set; }

    public Guid GameSessionCharacterId { get; set; }
    public GameSessionCharacter GameSessionCharacter { get; set; }
    /// <summary>
    /// Список способностей на перезарядке
    /// </summary>
    public List<AbilityCoolDowns> AbilityCoolDowns { get; set; }
}

public class AbilityCoolDowns
{
    public int AbilityId { get; set; }
    public int? LeftTurns { get; set; }
    public CharacterAbilityCoolDownTypeEnum CharacterAbilityCoolDownTypeEnum { get; set; }
}