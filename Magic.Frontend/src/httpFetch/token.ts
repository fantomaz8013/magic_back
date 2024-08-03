import httpFetch from "./index";
import {TokenRequest} from "../models/requests/tokenRequest";
import {TokenResponse} from "../models/responses/tokenResponse";

const prefix = 'token';

const endpoints = {
    getToken: prefix,
    refreshToken: prefix + "/refresh",
}

const token = {
    getToken: (r: TokenRequest) => {
        return httpFetch.post<TokenResponse>(endpoints.getToken, httpFetch.createRequestParams({...r}));
    },
    refreshToken: (token: string) => {
        return httpFetch.post<TokenResponse>(endpoints.refreshToken, httpFetch.createRequestParams(token));
    }
}

export default token;
