using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class TurnQueueResponse
{
    public List<Guid> GameSessionCharacterIds { get; set; }
    public int CurrentIndex { get; set; }
    public Guid GameSessionId { get; set; }

    public TurnQueueResponse(GameSessionCharacterTurnQueue turnQueue)
    {
        GameSessionCharacterIds = turnQueue.GameSessionCharacterIds;
        CurrentIndex = turnQueue.CurrentIndex;
        GameSessionId = turnQueue.GameSessionId;
    }
}