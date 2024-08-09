using Magic.Common.Models.Response;
using Magic.Domain.Entities;
using Magic.Domain.Enums;

namespace Magic.Api.Controllers.Websockets.Responses;

public record GameSessionInfo(
    GameSessionStatusTypeEnum GameSessionStatus,
    List<GameSessionCharacter>? Characters = null,
    MapResponse? Map = null
);