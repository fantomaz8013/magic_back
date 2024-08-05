import {BaseResponse} from "./baseResponse";

export type TokenResponse = BaseResponse<TokenResult>

interface TokenResult {
    tokenResult: {
        expires: string;
        expiresRefresh: string;
        refreshToken: string;
        role: string;
        token: string;
        userId: string;
    }
}
