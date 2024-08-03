export interface TokenResponse {
    tokenResult: {
        expires: string;
        expiresRefresh: string;
        refreshToken: string;
        role: string;
        token: string;
        userId: string;
    }
}
