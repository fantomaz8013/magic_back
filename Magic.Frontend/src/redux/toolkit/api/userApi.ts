import {createApi} from '@reduxjs/toolkit/query/react'
import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
import {UserResponse} from "../../../models/responses/userResponse";
import {HttpMethods} from "../../../httpFetch/httpMethods";
import UserUpdateRequest from "../../../models/requests/userUpdateRequest";

const prefix = 'user';

export const userApi = createApi({
    reducerPath: 'userApi',
    baseQuery: fetchBaseQueryWithAuth({baseUrl: prefix}),
    endpoints: (builder) => ({
        getCurrentUser: builder.query<UserResponse, void>({
            query: () => ``,
        }),
        updateCurrentUser: builder.query<UserResponse, UserUpdateRequest>({
            query: (params) => ({
                url: '',
                method: HttpMethods.POST,
                params: params
            }),
            onQueryStarted(params, {dispatch, queryFulfilled}) {
                queryFulfilled.then(({data}) => {
                    dispatch(userApi.util.updateQueryData(
                        'getCurrentUser',
                        undefined,
                        (draft) => {
                            draft.name = data.name;
                            draft.email = data.email;
                            draft.login = data.login;
                            draft.phoneNumber = data.phoneNumber;
                        }
                    ));
                });
            }
        }),
    }),
})

export const {useGetCurrentUserQuery, useUpdateCurrentUserQuery} = userApi
