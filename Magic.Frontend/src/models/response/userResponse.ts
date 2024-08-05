import {BaseResponse} from "./baseResponse";

export type UserResponse = BaseResponse<UserResult>;

export interface UserResult {
    name: string;
    id: string;
    login: string;
    email: string;
    phoneNumber: string;
}
