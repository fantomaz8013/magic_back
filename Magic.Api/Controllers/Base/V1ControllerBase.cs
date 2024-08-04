using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magic.Api.Controller.Base;

[ApiController]
[ApiVersion("1.0", Deprecated = false)]
[Route("api/v{version:apiVersion}/[action]")]
[Authorize]
public class V1ControllerBase : ControllerBase { }

[ApiExplorerSettings(GroupName = "User")]
public class V1UserControllerBase : V1ControllerBase { }
[ApiExplorerSettings(GroupName = "Character")]
public class V1CharacterControllerBase : V1ControllerBase { }

[ApiController]
[ApiVersion("1.0", Deprecated = false)]
[Route("api/v{version:apiVersion}/[action]")]
public class V1WithoutAuthorizeControllerBase : ControllerBase { }

[ApiExplorerSettings(GroupName = "UserRegister")]
public class V1UserRegisterControllerBase : V1WithoutAuthorizeControllerBase { }

[ApiExplorerSettings(GroupName = "Token")]
public class V1TokenControllerBase : V1WithoutAuthorizeControllerBase { }