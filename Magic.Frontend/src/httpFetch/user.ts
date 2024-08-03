import httpFetch from "./index";
import UserUpdateRequest from "../models/requests/userUpdateRequest";
import {UserResponse} from "../models/responses/userResponse";

export const prefix = 'user';

const endpoints = {
    getCurrentUser: prefix,
    updateCurrentUser: prefix,
}

const user = {
    getCurrentUser: () => {
        return httpFetch.get<UserResponse>(endpoints.getCurrentUser);
    },
    updateCurrentUser: (r: UserUpdateRequest) => {
        return httpFetch.put<boolean>(endpoints.updateCurrentUser, httpFetch.createRequestParams({...r}));
    }
}

export default user;
