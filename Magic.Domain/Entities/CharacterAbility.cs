using Magic.Domain.Enums;

namespace Magic.Domain.Entities;

/// <summary>
/// Внутриигровая способность персонажа
/// </summary>
public class CharacterAbility : BaseEntity<int>
{
    public const int ATTACK = 1;
    /// <summary>
    /// Название способности
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// Описание способности
    /// </summary>
    public string description {  get; set; }
    /// <summary>
    /// Дистанция применения способности
    /// </summary>
    public int? distance { get; set; }
    /// <summary>
    /// Радиус способности
    /// </summary>
    public int? radius {  get; set; }
    /// <summary>
    /// Тип способности
    /// </summary>
    public CharacterAbilityTypeEnum Type { get; set; }
    /// <summary>
    /// Тип применения способности
    /// </summary>
    public CharacterAbilityTargetTypeEnum TargetType { get; set; }
    /// <summary>
    /// Тип бросаемого кубика при применении способности
    /// </summary>
    public CubeTypeEnum? CubeType { get; set; }
    /// <summary>
    /// Тип действия
    /// </summary>
    public CharacterAbilityActionTypeEnum ActionType { get; set; }
    /// <summary>
    /// Тип перезарядки способности
    /// </summary>
    public CharacterAbilityCoolDownTypeEnum CoolDownType { get; set; }
    /// <summary>
    /// Количество ходов перезарядки, если соответствующий CoolDownType
    /// </summary>
    public int? coolDownCount { get; set; }
    /// <summary>
    /// Количество бросаемых кубиков
    /// </summary>
    public int? cubeCount { get; set; }
    public int? characterClassId { get; set; }
    /// <summary>
    /// Класс персонажа чья это способность. Если пусто, то способность может быть выбрана любым классом
    /// </summary>
    public CharacterClass CharacterClass { get; set; }
    public int? casterCharacterCharacteristicId { get; set; }
    /// <summary>
    /// Характеристика, по которой будет выполнена проверка у того, кто применяет заклинание
    /// </summary>
    public CharacterCharacteristic CasterCharacterCharacteristic { get; set; }
    public int? targetCharacterCharacteristicId { get; set; }
    /// <summary>
    /// Характеристика, по которой будет выполнена проверка у того, по кому применяют заклинание
    /// </summary>
    public CharacterCharacteristic TargetCharacterCharacteristic { get; set; }
}