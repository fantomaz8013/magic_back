import {CubeTypeEnum} from "../websocket/chatMessage";
import {BaseResponse} from "./baseResponse";

export type CharacterTemplateResponse = Omit<CharacterTemplate, 'characterClassId' | 'abilitieIds'>;

export interface CharacterTemplate extends BaseEntity<string> {
    name: string;
    description: string;
    avatarUrL: string;
    characterClass: CharacterClass;
    characterClassId: number;
    abilitieIds: number[];
    armor: number;
    characterRaceId: number;
    characterRace: CharacterRace;
    maxHP: number;
    speed: number;
    initiative: number;
    characteristics: CharacteristicsMapper;
}

export interface CharacterAbility extends BaseEntity<number> {
    title: string;
    icons: string;
    description: string;
    distance?: number;
    radius?: number;
    type: CharacterAbilityTypeEnum;
    targetType: CharacterAbilityTargetTypeEnum;
    cubeType?: CubeTypeEnum;
    actionType: CharacterAbilityActionTypeEnum;
    coolDownType: CharacterAbilityCoolDownTypeEnum;
    coolDownCount?: number;
    cubeCount?: number;
    characterClassId?: number;
    characterClass: CharacterClass;
    casterCharacterCharacteristicId?: number;
    casterCharacterCharacteristic: CharacterCharacteristic;
    targetCharacterCharacteristicId?: number;
    targetCharacterCharacteristic: CharacterCharacteristic;
    buffCount?: number;
    characterBuffId?: number;
    characterBuff?: GameSessionCharacterBuff;
}

export interface GameSessionCharacterBuff extends BaseEntity<number> {
    title: string;
    description: string;
    isNegative: boolean;
    buffType: BuffTypeEnum;
}

export enum BuffTypeEnum
{
    /// <summary>
    /// Нейтрализация персонажа
    /// </summary>
    Disable = 1,
    /// <summary>
    /// Полет
    /// </summary>
    Fly = 2,
    /// <summary>
    /// Добавить персонажу основное действие
    /// </summary>
    AddMainAction = 3,
    /// <summary>
    /// Внушение
    /// </summary>
    Suggestion = 4,
}

export type CharacteristicsMapper = { [key: number]: number };

export interface CharacterRace extends BaseEntity<number> {
    title: string;
    description: string;
}

export interface CharacterClass extends BaseEntity<number> {
    title: string;
    characterCharacteristicId: number;
}

export enum Classes {
    Warrior = 1,
    Wizard = 2,
    Hunter = 3,
    Priest = 4,
}

export enum CharacterAbilityTypeEnum {
    /// <summary>
    /// Атакующая способность
    /// </summary>
    Attack = 1,
    /// <summary>
    /// Защищающая способность
    /// </summary>
    Protection = 2,
    /// <summary>
    /// Лечащая способсность
    /// </summary>
    Healing = 3,
    /// <summary>
    /// Способность дарующая улучшения
    /// </summary>
    Buff = 4,
    /// <summary>
    /// Способность дарующая ухудшения
    /// </summary>
    DeBuff = 5,
}

export enum CharacterAbilityTargetTypeEnum {
    /// <summary>
    /// Способность должна применяться на конкретную цель
    /// </summary>
    Target = 1,
    /// <summary>
    /// Целью способности должна быть область
    /// </summary>
    Area = 2,
    /// <summary>
    /// Способность должна применяться на себя
    /// </summary>
    TargertSelf = 3,
    /// <summary>
    /// Способность должна применяться по конусу
    /// </summary>
    Cone = 4,
}

export enum CharacterAbilityActionTypeEnum {
    /// <summary>
    /// Основное действие
    /// </summary>
    MainAction = 1,
    /// <summary>
    /// Дополнительное действие
    /// </summary>
    AdditionalAction = 2,
}

export enum CharacterAbilityCoolDownTypeEnum {
    /// <summary>
    /// Способность перезаряжается во вермя драки.
    /// </summary>
    InFight = 1,
    /// <summary>
    /// Способность перезаряжается после драки
    /// </summary>
    AfterFight = 2,
    /// <summary>
    /// Способность не имеет перезарядку. Можно использовать каждый ход
    /// </summary>
    None = 3,
    /// <summary>
    /// Способность можно использовать 1 раз за всю игру
    /// </summary>
    OnePerGame = 4,
}

export enum CharacterRaces {
    Human = 1,
    Elf = 2,
    Dwarf = 3,
    Orc = 4,
}

export interface BaseEntity<T> {
    id: T;
}

export type CharacterCharacteristicResponse = BaseResponse<CharacterCharacteristic[]>;


export interface CharacterCharacteristic extends BaseEntity<number> {
    title: string;
    description: string;
}

export enum CharacterCharacteristicIds {
    Strength = 1,
    Agility = 2,
    Physique = 3,
    Intellect = 4,
    Wisdom = 5,
    Charisma = 6,
}
