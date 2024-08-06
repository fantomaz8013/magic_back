import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {TokenRequest} from "../../../models/request/tokenRequest";
import {setToken, getToken as getTokenLocalStorage} from "../../../utils/localStorage";
import {apiProxy} from "../../../env";
import {TokenResponse} from "../../../models/response/tokenResponse";
import {HttpMethods} from "../../../consts/httpMethods";
import {AuthResponse} from "../../../models/response/AuthResponse";

export interface AuthState {
    token: string | null;
    error: string | null;
    refreshToken: string | null;
}

const initialState: AuthState = {
    token: getTokenLocalStorage(),
    error: null,
    refreshToken: getTokenLocalStorage(true)
};

const slicePrefix = 'token';

export const refreshToken = createAsyncThunk(
    `${slicePrefix}/refresh`,
    async (refreshToken: string, {rejectWithValue}) => {
        return await parseFetch(fetch(apiProxy + `token/refresh`, createRequestParams(refreshToken)), rejectWithValue) as Promise<TokenResponse>
    }
);

export const getToken = createAsyncThunk(
    `${slicePrefix}/getToken`,
    async (r: TokenRequest, {rejectWithValue}) => {
        return await parseFetch(fetch(apiProxy + `token/byLogin`, createRequestParams({...r})), rejectWithValue) as Promise<AuthResponse>
    }
);

export const register = createAsyncThunk(
    `${slicePrefix}/register`,
    async (r: TokenRequest, {rejectWithValue}) => {
        return await parseFetch(fetch(apiProxy + `user/register`, createRequestParams({...r})), rejectWithValue) as Promise<TokenResponse>
    }
);

export const tokenSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        resetToken: (state) => {
            state.token = null;
            state.error = null;
            setToken(state.token);
        },
        resetError: (state) => {
            state.error = null;
        }
    },
    extraReducers: (builder) => builder
        .addCase(refreshToken.pending, (state) => {
            state.token = null;
            state.refreshToken = null;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(refreshToken.fulfilled, (state, action) => {
            state.token = action.payload.data!.token;
            state.refreshToken = action.payload.data!.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(refreshToken.rejected, (state, action) => {
            state.token = null;
            state.refreshToken = null;
            state.error = action.payload as string;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(getToken.fulfilled, (state, action) => {
            state.token = action.payload.data!.tokenResult!.token;
            state.refreshToken = action.payload.data!.tokenResult!.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(getToken.rejected, (state, action) => {
            state.error = action.payload as string;
        })
        .addCase(register.fulfilled, (state, action) => {
            state.token = action.payload.data!.token;
            state.refreshToken = action.payload.data!.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(register.rejected, (state, action) => {
            state.error = action.payload as string;
        })
});

//DON'T LOOK HERE IDIOT
function createRequestParams(body: Record<string, unknown> | string): RequestInit {
    return {
        headers: {
            "Content-Type": "application/json",
        },
        method: HttpMethods.POST,
        body: JSON.stringify(body)
    };
}

function parseFetch(promise: Promise<Response>, rejectWithValue: any) {
    return (promise
        .catch((error) => {
            if (!window.navigator.onLine) {
                console.error("Не можем подключиться к серверу");
            } else {
                console.error("Не можем подключиться к серверу. Попробуйте обновить страницу.");
            }

            throw error;
        })
        .then(value => {
            const response = value as Response;
            return response
                .json()
                .then(r => {
                    const tr = r as TokenResponse;
                    if (!tr.isSuccess)
                        return rejectWithValue(tr.errorText);
                    return tr;
                });
        }));
}

export const {resetToken, resetError} = tokenSlice.actions;
