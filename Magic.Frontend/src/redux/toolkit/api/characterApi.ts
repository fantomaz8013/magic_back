import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {apiProxy} from "../../../env";
import {
    CharacterCharacteristicResponse,
    CharacterTemplateResponse
} from "../../../models/response/characterTemplateResponse";
import {BaseResponse} from "../../../models/response/baseResponse";

const prefix = 'character';

export const characterApi = createApi({
    reducerPath: 'characterApi',
    baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
    endpoints: (builder) => ({
        getCharacterTemplates: builder.query<BaseResponse<CharacterTemplateResponse[]>, void>({
            query: () => `templates`,
        }),
        getCharacteristics: builder.query<CharacterCharacteristicResponse, void>({
            query: () => `characteristics`,
        }),
    }),
})

export const {useGetCharacterTemplatesQuery, useGetCharacteristicsQuery} = characterApi
