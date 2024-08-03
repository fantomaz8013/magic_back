import {combineReducers,} from "redux";
import {authSlice} from "./toolkit/slices/authSlice";
import {configureStore} from "@reduxjs/toolkit";
import {devMode} from "../env";
import {userApi} from "./toolkit/api/userApi";

export const rootReducer = combineReducers({
    auth: authSlice.reducer,
    [userApi.reducerPath]: userApi.reducer,
});

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({immutableCheck: false})
            .concat([
                userApi.middleware
            ]),
    devTools: devMode,
});

export type AppDispatch = typeof store.dispatch
export type RootState = ReturnType<typeof rootReducer>
