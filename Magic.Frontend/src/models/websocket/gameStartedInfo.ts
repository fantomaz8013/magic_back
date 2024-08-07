import {GameSessionStatusTypeEnum} from "./gameSessionStatus";
import {CharacterTemplate} from "../response/characterTemplateResponse";

export interface GameSessionInfo {
    characters?: GameSessionCharacter[];
    gameSessionStatus: GameSessionStatusTypeEnum;
}

export interface GameSessionCharacter extends CharacterTemplate {
    currentHP: number;
    currentShield?: number;
    positionX?: number;
    positionY?: number;
    ownerId: string;
    gameSessionId: string;
}
