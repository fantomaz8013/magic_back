namespace Magic.Api.Controllers.Websockets;

public static class Events
{
    public const string HistoryReceived = "historyReceived";
    public const string PlayerInfoReceived = "playerInfoReceived";
    public const string GameSessionInfoReceived = "gameSessionInfoReceived";
    public const string CharacterLocked = "characterLocked";
    public const string CharacterUnlocked = "characterUnlocked";
    public const string PlayerLeft = "playerLeft";
    public const string MessageReceived = "messageReceived";
    public const string GameStarted = "gameStarted";
}