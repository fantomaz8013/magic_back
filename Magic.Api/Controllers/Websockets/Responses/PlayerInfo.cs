namespace Magic.Api.Controllers.Websockets.Responses;

public record PlayerInfo(
    Guid Id,
    string Login,
    bool IsMaster,
    Guid? LockedCharacterId,
    bool IsOnline
);