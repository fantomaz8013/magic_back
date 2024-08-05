import {BaseResponse} from "./baseResponse";

export type TokenResponse = BaseResponse<TokenData>

export interface TokenData {
    expires: string;
    expiresRefresh: string;
    refreshToken: string;
    role: string;
    token: string;
    userId: string;
}
