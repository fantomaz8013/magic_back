using Magic.Domain.Entities;
using Magic.Service.Interfaces;
using System.Security.Claims;

namespace Magic.Service.Provider;

public class UserProvider : IUserProvider
{

    private readonly ITokenService _tokenService;
    private User? CurrentUser;
    private Guid? UserId;
    private Auth _auth;

    public UserProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public Guid? GetUserId()
    {
        if (UserId is null)
            UserId = _tokenService.GetUserId();
        return UserId;
    }

    public Guid? GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        if (UserId is null)
            UserId = _tokenService.GetUserId(claimsPrincipal);
        return UserId;
    }

    public Auth GetAuth()
    {
        if (_auth == null)
            _auth = _tokenService.GetAuth();
        return _auth;
    }
}