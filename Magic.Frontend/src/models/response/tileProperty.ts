import {BaseEntity} from "./characterTemplateResponse";

export interface TileProperty extends BaseEntity<number> {
    title: string;
    description: string;
    image: string;
    collisionType: TilePropertyCollisionTypeEnum;
    penaltyType: TilePropertyPenaltyTypeEnum;
    targetType: TilePropertyTargetTypeEnum;
    health: number | null;
    tilePropertyIdIfDestroyed: number | null;
    tilePropertyIfDestroyed: TileProperty | null;
    penaltyValue: number | null;
}

export enum TilePropertyCollisionTypeEnum {
    /// Свободный проход по клетке
    None = 1,
    /// Движение через этот тайл невозможно
    NoMove = 2,
    /// Движение только с полетом
    OnlyFly = 3,
}

export enum TilePropertyPenaltyTypeEnum {
    /// Нет штрафа за проход по клетке
    None = 1,
    /// Штраф в виде жизней
    PenaltyHealth = 2,
    /// Штраф в виде скорости
    PenaltySpeed = 3,
}

export enum TilePropertyTargetTypeEnum {
    /// Тайл нельзя выбрать в таргет
    None = 1,
    /// Разрушаемый
    Destructible = 2,
}
