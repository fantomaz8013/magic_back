export interface BaseResponse<T> {
    data: T | null;
    isSuccess: boolean;
    errorCode: string | null;
    errorText: string | null;
}
