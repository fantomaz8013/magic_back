using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magic.Api.Controller.Base;

[ApiController]
[Route("api/v1/[controller]/[action]")]
[Authorize]
public class V1ControllerBase : ControllerBase { }

public class V1UserControllerBase : V1ControllerBase { }
public class V1CharacterControllerBase : V1ControllerBase { }
public class V1MapControllerBase : V1ControllerBase { }
public class V1GameSessionControllerBase : V1ControllerBase { }

[ApiController]
[Route("api/v1/[controller]/[action]")]
public class V1WithoutAuthorizeControllerBase : ControllerBase { }

public class V1TokenControllerBase : V1WithoutAuthorizeControllerBase { }