namespace Magic.Api.Controllers.Websockets.Requests;

public record RequestedSaveThrow(
    int CharacterCharacteristicId,
    int Value,
    Guid CallerId,
    Guid UserId
);

public record RequestedSaveThrowPassed(
    int CharacterCharacteristicId,
    int Value,
    Guid CallerId,
    Guid UserId,
    int ResultRollValue
) : RequestedSaveThrow(
    CharacterCharacteristicId,
    Value,
    CallerId,
    UserId
)
{
    public RequestedSaveThrowPassed(RequestedSaveThrow requestedSaveThrow, int resultRollValue)
        : this(
            requestedSaveThrow.CharacterCharacteristicId,
            requestedSaveThrow.Value,
            requestedSaveThrow.CallerId,
            requestedSaveThrow.UserId,
            resultRollValue
        )
    {
    }
}