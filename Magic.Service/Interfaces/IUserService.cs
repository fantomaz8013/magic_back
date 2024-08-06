using Magic.Common.Models.Request;
using Magic.Common.Models.Response;

namespace Magic.Service;

public interface IUserService
{
    Task<AuthResponse?> Login(TokenRequest token);
    Task<TokenResponse?> RefreshToken(string token);
    Task<TokenResponse?> Register(UserRequest user);
    Task<UserResponse?> CurrentUser();
    Task<bool> UpdateUser(UserUpdateRequest request);
    Task<bool> UserExists(string login);
}