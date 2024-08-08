import {BaseEntity} from "./characterTemplateResponse";

export interface MapResponse extends BaseEntity<string> {
    tiles: number[][];
}
