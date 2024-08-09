import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {apiProxy} from "../../env";
import {EnterToGameSessionRequest} from "../../models/request/enterToGameSessionRequest";
import {BaseResponse} from "../../models/response/baseResponse";
import {GameSessionResponse} from "../../models/response/gameSessionResponse";
import {CreateGameSessionRequest} from "../../models/request/createGameSessionRequest";
import {HttpMethods} from "../../consts/httpMethods";

const prefix = 'gameSession';

export const gameSessionApi = createApi({
    reducerPath: `gameSessionApi`,
    tagTypes: ['GameSessions'],
    baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
    endpoints: (builder) => ({
        list: builder.query<BaseResponse<GameSessionResponse[]>, void>({
            query: () => ({
                url: `list`
            }),
            providesTags: ['GameSessions'],
        }),
        create: builder.mutation<BaseResponse<GameSessionResponse>, CreateGameSessionRequest>({
            query: (data) => ({
                url: `create`,
                method: HttpMethods.POST,
                body: data,
            }),
            invalidatesTags: ['GameSessions']
        }),
        enter: builder.mutation<BaseResponse<boolean>, EnterToGameSessionRequest>({
            query: (data) => ({
                url: `enter`,
                method: HttpMethods.POST,
                body: data
            }),
        }),
        deleteRequest: builder.mutation<BaseResponse<boolean>, EnterToGameSessionRequest>({
            query: (data) => ({
                url: `delete`,
                method: HttpMethods.DELETE,
                body: data
            }),
            invalidatesTags: ['GameSessions']
        }),
    }),
})

export const {
    useListQuery,
    useCreateMutation,
    useEnterMutation,
    useDeleteRequestMutation,
} = gameSessionApi
