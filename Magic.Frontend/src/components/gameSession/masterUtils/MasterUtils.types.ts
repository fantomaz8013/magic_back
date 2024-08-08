import {CharacterCharacteristicIds} from "../../../models/response/characterTemplateResponse";
import React from "react";
import {GameSessionCharacter} from "../../../models/websocket/gameStartedInfo";

export interface MasterToPlayerCommandProps {
    anchorEl: HTMLElement | null;
    onClose: () => void;
}

export enum MasterCommands {
    Kick = 'Kick',
    RequestSaveThrow = 'RequestSaveThrow',
    ChangeCharacteristics = 'ChangeCharacteristics',
}

export interface CharacteristicChangeDetails {
    type: CharacterCharacteristicIds;
    value: number;
}

export interface MasterCommandDetails {
    type: MasterCommands;
    userId: string;
}

export interface ChangingField {
    name: string;
    value: string;
    initialValue: string;
}

export interface FillableFieldInfo {
    name: string;
    type: React.InputHTMLAttributes<unknown>['type'];
}

export interface ChangingField {
    name: string;
    value: string;
    initialValue: string;
}


export type UnchangebleFields = Pick<GameSessionCharacter, 'gameSessionId' | 'abilities' | 'characterClass' | 'ownerId' | 'id' | 'characterRace'>;
export type ChangebleFields = keyof Omit<GameSessionCharacter, keyof UnchangebleFields>;
