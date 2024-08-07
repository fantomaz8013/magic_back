using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class GameSessionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxUserCount { get; set; }
    public Guid CreatorUserId { get; set; }
    public DateTime CreatedDate { get; set; }

    public GameSessionResponse(GameSession gameSession)
    {
        Id = gameSession.Id;
        Title = gameSession.Title;
        Description = gameSession.Description;
        MaxUserCount = gameSession.MaxUserCount;
        CreatorUserId = gameSession.CreatorUserId;
        CreatedDate = gameSession.CreatedDate;
    }
}