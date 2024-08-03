export default interface UserUpdateRequest {
    name?: string;
    phoneNumber?: string;
    email?: string;
    description?: string;
    gameExperience?: string;
    avatar?: File;
    cityId?: number;
}
