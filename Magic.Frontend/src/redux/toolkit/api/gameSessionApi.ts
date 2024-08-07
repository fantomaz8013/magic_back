import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {apiProxy} from "../../../env";
import {EnterToGameSessionRequest} from "../../../models/request/enterToGameSessionRequest";
import {BaseResponse} from "../../../models/response/baseResponse";

const prefix = 'gameSession';

export const gameSessionApi = createApi({
    reducerPath: `gameSessionApi`,
    baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
    endpoints: (builder) => ({
        enter: builder.mutation<BaseResponse<boolean>, EnterToGameSessionRequest>({
            query: () => `enter`,
        }),
        create: builder.mutation<BaseResponse<boolean>, EnterToGameSessionRequest>({
            query: () => `create`,
        }),
    }),
})

export const {} = gameSessionApi
