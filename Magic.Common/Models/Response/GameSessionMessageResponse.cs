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
    public Guid Id { get; set; }
    public Guid GameSessionId { get; set; }
    public DateTime CreatedDate { get; set; }

    public BaseGameSessionMessageResponse(BaseGameSessionMessage message)
    {
        Id = message.Id;
        CreatedDate = message.CreatedDate;
        GameSessionId = message.GameSessionId;
    }
}

public class ChatGameGameSessionMessageResponse : ServerGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Chat;
    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }

    public ChatGameGameSessionMessageResponse(ChatGameGameSessionMessage chatGameGameSessionMessage)
        : base(chatGameGameSessionMessage)
    {
        AuthorId = chatGameGameSessionMessage.AuthorId;
        Author = new UserResponse(chatGameGameSessionMessage.Author);
    }
}

public class ServerGameSessionMessageResponse : BaseGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Server;

    public string Message { get; set; }

    public ServerGameSessionMessageResponse(ServerGameSessionMessage serverGameSessionMessage)
        : base(serverGameSessionMessage)
    {
        Message = serverGameSessionMessage.Message;
    }
}

public class DiceGameSessionMessageResponse : BaseGameSessionMessageResponse
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Dice;
    public int Roll { get; set; }
    public CubeTypeEnum CubeTypeEnum { get; set; }

    public Guid AuthorId { get; set; }
    public UserResponse Author { get; set; }

    public DiceGameSessionMessageResponse(DiceGameSessionMessage diceGameSessionMessage) : base(diceGameSessionMessage)
    {
        Roll = diceGameSessionMessage.Roll;
        CubeTypeEnum = diceGameSessionMessage.CubeTypeEnum;
        AuthorId = diceGameSessionMessage.AuthorId;
        Author = new UserResponse(diceGameSessionMessage.Author);
    }
}