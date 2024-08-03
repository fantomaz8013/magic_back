import httpFetch from "./index";
import UserRequest from "../models/requests/userRequest";
import {prefix} from "./user";
import {TokenResponse} from "../models/responses/tokenResponse";

const endpoints = {
    register: prefix + "/register",
}


const userRegister = {
    register: (r: UserRequest) => {
        return httpFetch.post<TokenResponse>(endpoints.register, httpFetch.createRequestParams({...r}));
    }
}

export default userRegister;
