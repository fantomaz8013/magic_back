namespace Magic.Domain.Entities;

public class GameSessionCharacterTurnQueue : BaseEntity<Guid>
{
    public List<Guid> GameSessionCharacterIds { get; set; }
    public int CurrentIndex { get; set; }
    public Guid GameSessionId { get; set; }
    public GameSession GameSession { get; set; }
}