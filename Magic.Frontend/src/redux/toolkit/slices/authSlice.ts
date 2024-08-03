import {createAsyncThunk, createSlice, PayloadAction} from "@reduxjs/toolkit";
import httpFetch from "../../../httpFetch";
import {TokenRequest} from "../../../models/requests/tokenRequest";

export interface AuthState {
    token: string | null;
}

const initialState: AuthState = {
    token: null
};

const slicePrefix = 'auth';

export const refreshToken = createAsyncThunk(
    `${slicePrefix}/refreshToken`,
    async (token: string) => {
        return await httpFetch.token.refreshToken(token);
    }
);

export const login = createAsyncThunk(
    `${slicePrefix}/login`,
    async (r: TokenRequest) => {
        return await httpFetch.token
            .getToken(r)
    }
);

export const register = createAsyncThunk(
    `${slicePrefix}/register`,
    async (r: TokenRequest) => {
        return await httpFetch.userRegister
            .register(r)
    }
);

export const authSlice = createSlice({
    name: slicePrefix,
    initialState,
    reducers: {
        logout: (state) => {
            state.token = null;
        }
    },
    extraReducers: (builder) => builder
        .addCase(refreshToken.pending, (state) => {
            state.token = null;
        })
        .addCase(refreshToken.fulfilled, (state, action) => {
            state.token = action.payload.tokenResult.token;
        })
        .addCase(login.fulfilled, (state, action) => {
            state.token = action.payload.tokenResult.token;
        })
        .addCase(register.fulfilled, (state, action) => {
            state.token = action.payload.tokenResult.token;
        })
});

export const {logout} = authSlice.actions;
