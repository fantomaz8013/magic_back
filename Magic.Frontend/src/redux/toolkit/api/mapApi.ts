import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {apiProxy} from "../../../env";
import {BaseResponse} from "../../../models/response/baseResponse";
import {MapResponse} from "../../../models/response/mapResponse";
import {TileProperty} from "../../../models/response/tileProperty";

const prefix = 'map';

export const mapApi = createApi({
    reducerPath: 'mapApi',
    baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
    endpoints: (builder) => ({
        getMapList: builder.query<BaseResponse<MapResponse>, void>({
            query: () => `list`,
        }),
        getTileProperties: builder.query<BaseResponse<TileProperty[]>, void>({
            query: () => `tileProperties`,
        }),
    }),
})

export const {useGetMapListQuery, useGetTilePropertiesQuery} = mapApi
