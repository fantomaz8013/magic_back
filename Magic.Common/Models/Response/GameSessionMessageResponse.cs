using System.Text.Json.Serialization;
using Magic.Domain.Entities;
using Magic.Domain.Enums;

namespace Magic.Common.Models.Response;

[JsonDerivedType(typeof(ChatGameGameSessionMessageResponse))]
[JsonDerivedType(typeof(DiceGameSessionMessageResponse))]
[JsonDerivedType(typeof(ServerGameSessionMessageResponse))]
public abstract class BaseGameSessionMessageResponse
{
    public virtual GameSessionMessageTypeEnum GameSessionMessageTypeEnum { get; set; }
    public Guid GameSessionId { get; set; }
    public DateTime CreatedDate { get; set; }

    public BaseGameSessionMessageResponse(DateTime createdDate, Guid gameSessionId)
    {
        CreatedDate = createdDate;
        GameSessionId = gameSessionId;
    }
}

public class ChatGameGameSessionMessageResponse : ServerGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Chat;
    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }

    public ChatGameGameSessionMessageResponse(Guid authorId, UserResponse author, string message, DateTime createdDate,
        Guid gameSessionId) : base(message, createdDate, gameSessionId)
    {
        AuthorId = authorId;
        Author = author;
    }

    public static ChatGameGameSessionMessageResponse BuildResponse(
        ChatGameGameSessionMessage chatGameGameSessionMessage)
    {
        return new ChatGameGameSessionMessageResponse(
            chatGameGameSessionMessage.AuthorId,
            UserResponse.BuildResponse(chatGameGameSessionMessage.Author),
            chatGameGameSessionMessage.Message, chatGameGameSessionMessage.CreatedDate,
            chatGameGameSessionMessage.GameSessionId
        );
    }
}

public class ServerGameSessionMessageResponse : BaseGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Server;
    public string Message { get; set; }

    public ServerGameSessionMessageResponse(string message, DateTime createdDate, Guid gameSessionId) : base(
        createdDate, gameSessionId)
    {
        Message = message;
    }

    public static ServerGameSessionMessageResponse BuildResponse(ServerGameSessionMessage serverGameSessionMessage)
    {
        return new ServerGameSessionMessageResponse(
            serverGameSessionMessage.Message,
            serverGameSessionMessage.CreatedDate,
            serverGameSessionMessage.GameSessionId
        );
    }
}

public class DiceGameSessionMessageResponse : BaseGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Dice;
    public int Roll { get; set; }
    public CubeTypeEnum CubeTypeEnum { get; set; }

    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }

    public DiceGameSessionMessageResponse(DateTime createdDate, Guid gameSessionId, int roll, CubeTypeEnum cubeTypeEnum,
        Guid authorId, UserResponse author) : base(
        createdDate, gameSessionId)
    {
        Roll = roll;
        CubeTypeEnum = cubeTypeEnum;
        AuthorId = authorId;
        Author = author;
    }

    public static DiceGameSessionMessageResponse BuildResponse(DiceGameSessionMessage diceGameSessionMessage)
    {
        return new DiceGameSessionMessageResponse(
            diceGameSessionMessage.CreatedDate,
            diceGameSessionMessage.GameSessionId,
            diceGameSessionMessage.Roll,
            diceGameSessionMessage.CubeTypeEnum,
            diceGameSessionMessage.AuthorId,
            UserResponse.BuildResponse(diceGameSessionMessage.Author)
        );
    }
}