import {fetchBaseQuery, FetchBaseQueryArgs} from '@reduxjs/toolkit/query';
import type {BaseQueryFn, FetchArgs, FetchBaseQueryError} from '@reduxjs/toolkit/query';
import {Mutex} from 'async-mutex';
import {RootState} from "../../index";
import {refreshToken} from "../slices/tokenSlice";
import {apiProxy} from "../../../env";

const mutex = new Mutex();

export const fetchBaseQueryWithAuth = (fetchArgs?: FetchBaseQueryArgs | undefined): BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> => {
    const baseQuery = fetchBaseQuery({
        ...fetchArgs,
        baseUrl: apiProxy + (fetchArgs?.baseUrl || ''),
        prepareHeaders: (headers, api) => {
            const rootState = api.getState() as RootState;
            const token = rootState.auth.token;
            if (token) {
                headers.set('authorization', `Bearer ${token}`);
            }
            if (fetchArgs?.prepareHeaders) {
                return fetchArgs?.prepareHeaders(headers, api);
            }
            return headers;
        }
    });

    return async (args, api, extraOptions) => {

        await mutex.waitForUnlock();

        const state = api.getState() as RootState;

        const result = await baseQuery(args, api, extraOptions);
        if (!result.error || result.error.status !== 401 || !state.auth.refreshToken) {
            return result;
        }

        if (mutex.isLocked()) {
            await mutex.waitForUnlock();
            return baseQuery(args, api, extraOptions);
        }

        const release = await mutex.acquire();
        try {
            const token = await api.dispatch(refreshToken(state.auth.refreshToken)).unwrap();
            if (token) {
                return baseQuery(args, api, extraOptions);
            }
        } finally {
            release();
        }

        return result;
    };
};
