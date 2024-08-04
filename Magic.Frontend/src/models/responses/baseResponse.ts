export interface BaseResponse<T> {
    data: T;
    isSuccess: boolean;
    errorCode: string | null;
    errorText: string | null;
}
