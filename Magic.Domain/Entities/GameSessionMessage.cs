using Magic.Domain.Enums;

namespace Magic.Domain.Entities;

public abstract class BaseGameSessionMessage : BaseEntity<Guid>, IHasCreatedDate
{
    public virtual GameSessionMessageTypeEnum GameSessionMessageTypeEnum { get; set; }
    public Guid GameSessionId { get; set; }
    public GameSession GameSession { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class ChatGameGameSessionMessage : ServerGameSessionMessage
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Chat;
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
}

public class ServerGameSessionMessage : BaseGameSessionMessage
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Server;
    public string Message { get; set; }
}

public class DiceGameSessionMessage : BaseGameSessionMessage
{
    public override GameSessionMessageTypeEnum GameSessionMessageTypeEnum => GameSessionMessageTypeEnum.Dice;
    public int Roll { get; set; }
    public CubeTypeEnum CubeTypeEnum { get; set; }

    public Guid AuthorId { get; set; }
    public User Author { get; set; }
}