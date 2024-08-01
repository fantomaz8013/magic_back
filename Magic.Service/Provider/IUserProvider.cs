using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Service.Provider
{
    public interface IUserProvider
    {
        Guid? GetUserId();
        Guid? GetUserId(ClaimsPrincipal claimsPrincipal);
        Auth GetAuth();
    }
}
