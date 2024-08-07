import {BaseEntity} from "./characterTemplateResponse";

export interface GameSessionResponse extends BaseEntity<string> {
    title: string;
    description: string;
    maxUserCount: number;
    creatorUserId: string;
    createdDate: string;
}
