import {GameSessionStatusTypeEnum} from "./gameSessionStatus";
import {CharacterTemplate} from "../response/characterTemplateResponse";
import {MapResponse} from "../response/mapResponse";

export interface GameSessionInfo {
    characters?: GameSessionCharacter[];
    gameSessionStatus: GameSessionStatusTypeEnum;
    map: MapResponse;
}

export interface GameSessionCharacter extends CharacterTemplate {
    currentHP: number;
    currentShield?: number;
    positionX?: number;
    positionY?: number;
    ownerId: string;
    gameSessionId: string;
}
