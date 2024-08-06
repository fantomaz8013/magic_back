// import {createApi} from '@reduxjs/toolkit/query/react'
// import {fetchBaseQueryWithAuth} from "../utils/baseQueryWithReauth";
// import {UserResponse} from "../../../models/response/userResponse";
// import {apiProxy} from "../../../env";
//
// const prefix = 'character';
//
// export const characterApi = createApi({
//     reducerPath: 'characterApi',
//     baseQuery: fetchBaseQueryWithAuth(apiProxy + prefix),
//     endpoints: (builder) => ({
//         getCharacterTemplates: builder.query<UserResponse, void>({
//             query: () => `templates`,
//         }),
//         // updateCurrentUser: builder.query<UserResponse, UserUpdateRequest>({
//         //     query: (params) => ({
//         //         url: '',
//         //         method: HttpMethods.POST,
//         //         params: params
//         //     }),
//         //     onQueryStarted(params, {dispatch, queryFulfilled}) {
//         //         queryFulfilled.then(({response}) => {
//         //             dispatch(userApi.util.updateQueryData(
//         //                 'getCurrentUser',
//         //                 undefined,
//         //                 (draft) => {
//         //                     draft.name = response.data.name;
//         //                     draft.email = response.data.email;
//         //                     draft.login = response.data.login;
//         //                     draft.phoneNumber = response.data.phoneNumber;
//         //                 }
//         //             ));
//         //         });
//         //     }
//         // }),
//     }),
// })
//
// export const {useGetCurrentUserQuery} = userApi
