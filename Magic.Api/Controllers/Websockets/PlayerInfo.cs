namespace Magic.Api.Controllers.Websockets;

public record PlayerInfo(Guid Id, string Login, bool IsMaster, Guid? LockedCharacterId);