import {BaseEntity} from "./characterTemplateResponse";
import {MapResponse} from "./mapResponse";

export interface GameSessionResponse extends BaseEntity<string> {
    title: string;
    description: string;
    maxUserCount: number;
    currentUserCount: number;
    creatorUserId: string;
    createdDate: string;
    map: MapResponse | null;
}
