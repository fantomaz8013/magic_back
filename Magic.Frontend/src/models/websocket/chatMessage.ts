import {BaseEntity} from "../response/characterTemplateResponse";

export interface BaseGameSessionMessage extends BaseEntity<string> {
    gameSessionId: string;
    createdDate: string;
    gameSessionMessageTypeEnum: GameSessionMessageTypeEnum;
}

export type ChatGameSessionMessage = Omit<ServerGameSessionMessage, 'gameSessionMessageTypeEnum'> & WithAuthor & {

    gameSessionMessageTypeEnum: GameSessionMessageTypeEnum.Chat;
}

export interface ServerGameSessionMessage extends BaseGameSessionMessage {
    message: string;
    gameSessionMessageTypeEnum: GameSessionMessageTypeEnum.Server;
}

export interface WithAuthor {
    authorId: string;
    author: User;
}

export type DiceGameSessionMessage = BaseGameSessionMessage & WithAuthor & {
    roll: number;
    cubeTypeEnum: CubeTypeEnum;
    gameSessionMessageTypeEnum: GameSessionMessageTypeEnum.Dice;
}

export interface User {
    login: string;
}

export enum GameSessionMessageTypeEnum {
    Server,
    Chat,
    Dice,
}

export enum CubeTypeEnum {
    /// <summary>
    /// Кубик с 4 гранями
    /// </summary>
    D4 = 1,
    /// <summary>
    /// Кубик с 6 гранями
    /// </summary>
    D6 = 2,
    /// <summary>
    /// Кубик с 8 гранями
    /// </summary>
    D8 = 3,
    /// <summary>
    /// Кубик с 10 гранями
    /// </summary>
    D10 = 4,
    /// <summary>
    /// Кубик с 12 гранями
    /// </summary>
    D12 = 5,
    /// <summary>
    /// Кубик с 20 гранями
    /// </summary>
    D20 = 6,
}
