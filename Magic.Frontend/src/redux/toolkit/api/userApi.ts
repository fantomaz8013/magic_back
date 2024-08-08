import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {UserResponse} from "../../../models/response/userResponse";
import {apiProxy} from "../../../env";
import {BaseResponse} from "../../../models/response/baseResponse";

const prefix = 'user';

export const userApi = createApi({
    reducerPath: 'userApi',
    baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
    endpoints: (builder) => ({
        getCurrentUser: builder.query<BaseResponse<UserResponse>, void>({
            query: () => `currentUser`,
        }),
    }),
})

export const {useGetCurrentUserQuery} = userApi
