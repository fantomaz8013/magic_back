using Magic.Common.Models.Request;
using Magic.Common.Models.Response;
using Magic.DAL.Dto.Implementations;
using Magic.Domain.Entities;

namespace Magic.Service
{
    public interface IUserService : IBaseEntityService<User, UserDto>
    {
        Task<AuthResponse?> Login(TokenRequest token);
        Task<TokenResponse?> RefreshToken(string token);
        Task<TokenResponse?> Register(UserRequest user);
        Task<UserResponse?> CurrentUser();
        Task<bool> UserExists(string login);
    }
}
