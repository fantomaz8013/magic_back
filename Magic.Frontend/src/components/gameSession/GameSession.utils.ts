import {Navigate} from "react-router-dom";
import paths from "../../consts/paths";
import React from "react";
import {PlayerInfo} from "./GameSession";
import {UserResponse} from "../../models/response/userResponse";

export const SavingThrowEnum = {
    VeryEasy: 5,
    Easy: 10,
    Medium: 15,
    Hard: 20,
    VeryHard: 25,
    Impossible: 30,
}

export const SavingThrowEnumMapper = {
    [SavingThrowEnum.VeryEasy]: 'Очень лёгкая',
    [SavingThrowEnum.Easy]: 'Лёгкая',
    [SavingThrowEnum.Medium]: 'Средняя',
    [SavingThrowEnum.Hard]: 'Сложная',
    [SavingThrowEnum.VeryHard]: 'Очень сложная',
    [SavingThrowEnum.Impossible]: 'Практически невозможная',
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
