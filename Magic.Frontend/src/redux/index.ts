import {combineReducers,} from "redux";
import {tokenSlice} from "./toolkit/slices/tokenSlice";
import {configureStore} from "@reduxjs/toolkit";
import {devMode} from "../env";
import {userApi} from "./toolkit/api/userApi";
import {characterApi} from "./toolkit/api/characterApi";
import {gameSessionSlice} from "./toolkit/slices/gameSessionSlice";

export const rootReducer = combineReducers({
    auth: tokenSlice.reducer,
    gameSession: gameSessionSlice.reducer,
    [userApi.reducerPath]: userApi.reducer,
    [characterApi.reducerPath]: characterApi.reducer,
});

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({immutableCheck: false})
            .concat([
                userApi.middleware,
                characterApi.middleware
            ]),
    devTools: devMode,
});

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof rootReducer>
