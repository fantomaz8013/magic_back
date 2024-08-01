using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Magic.Common.Models.Response;
using Magic.Common.Models.Request;
using Magic.DAL.Dto.Implementations;
using Magic.Domain.Entities;
using Magic.DAL.Extensions;
using Magic.DAL;
using Magic.Service.Provider;
using System.IdentityModel.Tokens.Jwt;
using Magic.Service.Interfaces;

namespace Magic.Service
{
    public class UserService : IUserService
    {
        protected readonly DataBaseContext _dbContext;
        protected readonly ITokenService _jwtTokenService;
        protected readonly IUserProvider _userProvider;
        protected readonly ILogProvider _logProvider;
        public UserService(DataBaseContext dbContext, ITokenService jwtTokenService, IUserProvider userProvider, ILogProvider logProvider)
        {
            _dbContext = dbContext;
            _jwtTokenService = jwtTokenService;
            _userProvider = userProvider;
            _logProvider = logProvider;
        }
        
        public async Task<bool> UserExists(string login)
        {
            if (await _dbContext.User.AnyAsync(x => x.Login == login))
                return true;
            return false;
        }

        public async Task<TokenResponse?> Register(UserRequest user)
        {
            if (await UserExists(user.Login))
                throw new Exception("Логин занят.");

            string passwordHash, passwordSalt;
            CreatePasswordHash(user.Password, out passwordHash, out passwordSalt);

            var newUser = new User
            {
                Login = user.Login,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsBlocked = false,
                CreatedDate = DateTime.UtcNow,
            };

            await _dbContext.User.AddAsync(newUser);

            await _dbContext.SaveChangesAsync();

            return _jwtTokenService.CreateUserToken(newUser);
        }

        public async Task<AuthResponse?> Login(TokenRequest token)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Login == token.Login);

            if (user == null) 
                return null;

            if (user.IsBlocked)
                return null;

            var authResponseModel = new AuthResponse();

            if (token.Password != null)
            {
                if (token.Password != null && !VerifyPassword(token.Password, user.PasswordHash, user.PasswordSalt))
                    return null;

                authResponseModel.TokenResult = _jwtTokenService.CreateUserToken(user);
            }

            await _logProvider.WriteInformation($"{user.Name} get token");

            return authResponseModel;
    }

        public async Task<TokenResponse> RefreshToken(string refreshToken)
        {
            _jwtTokenService.ValidateRefreshToken(refreshToken);

            var tokenInfo = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
            var userId = _jwtTokenService.GetEntityPublicIdOrThrow(tokenInfo.Claims);
            var exp = tokenInfo.Claims.First(x => x.Type == "exp").Value;

            var date = DateTimeOffset.FromUnixTimeSeconds(long.Parse(exp));
            var user = await _dbContext
                .User
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                //_errorService.UserNotFound();
                return null!; // hint: это только для IDE
            }

            if (user.IsBlocked)
                return null;

            return _jwtTokenService.CreateUserToken(user, date.Date);
        }

        public async Task<UserResponse?> CurrentUser()
        {
            var auth = _userProvider.GetAuth();
            Guid? userId = _userProvider.GetUserId();

            var user = await _dbContext.User.FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) 
                return null;
            else
            {
                return new UserResponse(user.Name, user.Login, user.Email, user.PhoneNumber);
            }
        }


        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }

        private bool VerifyPassword(string password, string passwordHash, string passwordSalt)
        {
            using (var hmac = new HMACSHA512(Convert.FromBase64String(passwordSalt)))
            {
                var passwordHashByteArray = Convert.FromBase64String(passwordHash);
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHashByteArray[i]) return false;
                }
            }
            return true;
        }

        private const string _prefix = "";
        private const int _numberOfSecureBytesToGenerate = 32;
        private const int _lengthOfKey = 36;
        private string GenerateApiSecret()
        {
            var bytes = RandomNumberGenerator.GetBytes(_numberOfSecureBytesToGenerate);

            return string.Concat(_prefix, Convert.ToBase64String(bytes)
                                .Replace("/", "")
                                .Replace("+", "")
                                .Replace("=", "")
                                .AsSpan(0, _lengthOfKey - _prefix.Length));
        }

        public async Task<UserDto?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
            //return await _dbContext.User
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PagedResult<UserDto>> ListAsync(PagedRequest request)
        {
            return await _dbContext.User
                .AsNoTracking()
                .ApplyRequestAsync(request, x => new UserDto(x));
        }

        public async Task<IEnumerable<UserDto>> ListAsync(Expression<Func<User, bool>> predicate)
        {
            throw new NotImplementedException();
            //return await _dbContext.User
            //    .AsNoTracking()
            //    .Where(predicate)
            //    .ToListAsync();

        }
    }
}
