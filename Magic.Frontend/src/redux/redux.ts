import {combineReducers,} from "redux";
import {tokenSlice} from "./slices/tokenSlice";
import {configureStore} from "@reduxjs/toolkit";
import {devMode} from "../env";
import {userApi} from "./api/userApi";
import {characterApi} from "./api/characterApi";
import {gameSessionSlice} from "./slices/gameSessionSlice";
import {gameSessionApi} from "./api/gameSessionApi";
import {mapApi} from "./api/mapApi";

export const rootReducer = combineReducers({
    auth: tokenSlice.reducer,
    gameSession: gameSessionSlice.reducer,
    [userApi.reducerPath]: userApi.reducer,
    [mapApi.reducerPath]: mapApi.reducer,
    [characterApi.reducerPath]: characterApi.reducer,
    [gameSessionApi.reducerPath]: gameSessionApi.reducer,
});

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({immutableCheck: false})
            .concat([
                userApi.middleware,
                characterApi.middleware,
                gameSessionApi.middleware,
                mapApi.middleware,
            ]),
    devTools: devMode,
});

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof rootReducer>
