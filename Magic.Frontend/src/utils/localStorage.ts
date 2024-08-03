import {PaletteMode} from "@mui/material";
import UserRequest from "../models/requests/userRequest";

const keys = {
    mode: 'mode',
    account: 'account'
}

export function setPaletteMode(mode: PaletteMode) {
    localStorage.setItem(keys.mode, mode);
}

export function setAccount(r: UserRequest) {
    localStorage.setItem(keys.account, JSON.stringify(r));
}

export function getPaletteMode(): PaletteMode {
    const val = localStorage.getItem(keys.mode);
    if (val)
        return val as PaletteMode;
    return 'light';
}

export function getAccount() {
    const val = localStorage.getItem(keys.account);
    if (val)
        return JSON.parse(val) as UserRequest;
    return null;
}
