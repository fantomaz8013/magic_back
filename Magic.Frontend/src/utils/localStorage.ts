import {PaletteMode} from "@mui/material";

const keys = {
    mode: 'mode',
    token: 'token',
    refreshToken: 'refreshToken'
}

export function setPaletteMode(mode: PaletteMode) {
    localStorage.setItem(keys.mode, mode);
}

export function setToken(token: string | null, isRefresh = false) {
    const key = isRefresh ? keys.refreshToken : keys.token;
    if (token)
        localStorage.setItem(key, token);
    else
        localStorage.removeItem(key);
}

export function getPaletteMode(): PaletteMode {
    const val = localStorage.getItem(keys.mode);
    if (val)
        return val as PaletteMode;
    return 'light';
}

export function getToken(isRefresh = false) {
    const key = isRefresh ? keys.refreshToken : keys.token;
    const val = localStorage.getItem(key);
    if (val)
        return val;
    return null;
}
