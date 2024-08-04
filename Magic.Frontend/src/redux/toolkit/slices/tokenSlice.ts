import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {TokenRequest} from "../../../models/requests/tokenRequest";
import {setToken, getToken as getTokenLocalStorage} from "../../../utils/localStorage";
import {apiProxy} from "../../../env";
import {TokenResponse} from "../../../models/responses/tokenResponse";
import {HttpMethods} from "../../../consts/httpMethods";

export interface AuthState {
    token: string | null;
    refreshToken: string | null;
}

const initialState: AuthState = {
    token: getTokenLocalStorage(),
    refreshToken: getTokenLocalStorage(true)
};

const slicePrefix = 'token';

export const refreshToken = createAsyncThunk(
    `${slicePrefix}/refreshToken`,
    async (token: string, {rejectWithValue}) => {
        return parseFetch(fetch(apiProxy + `token/refreshToken`, createRequestParams({token})), rejectWithValue)
    }
);

export const getToken = createAsyncThunk(
    `${slicePrefix}/getToken`,
    async (r: TokenRequest, {rejectWithValue}) => {
        return await parseFetch(fetch(apiProxy + `token`, createRequestParams({...r})), rejectWithValue)
    }
);

export const register = createAsyncThunk(
    `${slicePrefix}/register`,
    async (r: TokenRequest, {rejectWithValue}) => {
        return await parseFetch(fetch(apiProxy + `user/register`, createRequestParams({...r})), rejectWithValue)
    }
);

export const tokenSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        resetToken: (state) => {
            state.token = null;
            setToken(state.token);
        }
    },
    extraReducers: (builder) => builder
        .addCase(refreshToken.pending, (state) => {
            state.token = null;
            state.refreshToken = null;
            setToken(state.token);
        })
        .addCase(refreshToken.fulfilled, (state, action) => {
            state.token = action.payload.data!.tokenResult.token;
            state.refreshToken = action.payload.data!.tokenResult.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(getToken.fulfilled, (state, action) => {
            state.token = action.payload.data!.tokenResult.token;
            state.refreshToken = action.payload.data!.tokenResult.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
        })
        .addCase(register.fulfilled, (state, action) => {
            state.token = action.payload.data!.tokenResult.token;
            state.refreshToken = action.payload.data!.tokenResult.refreshToken;
            setToken(state.token);
            setToken(state.refreshToken, true);
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
        }) as Promise<TokenResponse>);
}

export const {resetToken} = tokenSlice.actions;
