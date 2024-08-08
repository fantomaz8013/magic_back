namespace Magic.Api.Controllers.Websockets;

public static class Events
{
    //mesages
    public const string HistoryReceived = "historyReceived";
    public const string MessageReceived = "messageReceived";

    //global info
    public const string PlayerInfoReceived = "playerInfoReceived";
    public const string GameSessionInfoReceived = "gameSessionInfoReceived";


    //characters
    public const string CharacterLocked = "characterLocked";
    public const string CharacterUnlocked = "characterUnlocked";

    //player
    public const string PlayerLeft = "playerLeft";
    public const string PlayerSaveThrow = "playerSaveThrow";
    public const string PlayerSaveThrowPassed = "playerSaveThrowPassed";
}