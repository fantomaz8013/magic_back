import {ChangebleFields, CharacteristicChangeDetails, FillableFieldInfo} from "./MasterUtils.types";
import {CharacterCharacteristicIds} from "../../../models/response/characterTemplateResponse";
import {PlayerInfo} from "../GameSession";

export const fillableFields: Record<ChangebleFields, FillableFieldInfo> = {
    name: {
        name: 'Имя',
        type: 'text',
    },
    characteristics: {
        name: 'Характеристики',
        type: 'text',
    },
    characterClassId: {
        name: 'ID класса',
        type: 'number',
    },
    characterRaceId: {
        name: 'ID рассы',
        type: 'number',
    },
    description: {
        name: 'Описание',
        type: 'text',
    },
    armor: {
        name: 'Класс брони',
        type: 'number',
    },
    avatarUrL: {
        name: 'Аватар',
        type: 'text',
    },
    abilitieIds: {
        name: 'Способности',
        type: 'text',
    },
    currentHP: {
        name: 'Текущий уровень здоровья',
        type: 'number',
    },
    currentShield: {
        name: 'Щит',
        type: 'number',
    },
    initiative: {
        name: 'Инициатива',
        type: 'number',
    },
    maxHP: {
        name: 'Максимальный уровень здоровья',
        type: 'number',
    },
    positionX: {
        name: 'Позиция X',
        type: 'number',
    },
    positionY: {
        name: 'Позиция Y',
        type: 'number',
    },
    speed: {
        name: 'Скорость',
        type: 'number',
    },
}

export const ModalStyle = {
    position: 'absolute' as 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    border: '2px solid #000',
    boxShadow: 24,
    p: 4,
};

export const defaultCharacteristic: CharacteristicChangeDetails = {
    type: CharacterCharacteristicIds.Strength,
    value: 5
};

export function getUserLoginById(playerInfos: Record<string, PlayerInfo>, userId: string) {
    return playerInfos[userId].login;
}
