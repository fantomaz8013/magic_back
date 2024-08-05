import {BaseResponse} from "./baseResponse";
import {TokenData} from "./tokenResponse";

export type AuthResponse = BaseResponse<{
    tokenResult?: TokenData;
    isNeedEnterCode?: boolean;
    confirmCodeLifeTime: number;
}>;
