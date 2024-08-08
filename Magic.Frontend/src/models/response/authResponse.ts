import {TokenResponse} from "./tokenResponse";

export interface AuthResponse {
    tokenResult?: TokenResponse;
    isNeedEnterCode?: boolean;
    confirmCodeLifeTime: number;
}
