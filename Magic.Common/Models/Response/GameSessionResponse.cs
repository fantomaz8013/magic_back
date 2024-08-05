namespace Magic.Common.Models.Response;

public class GameSessionResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxUserCount { get; set; }
    public Guid CreatorUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public GameSessionResponse(Guid id, string title, string description, int maxUserCount, Guid creatorUserId, DateTime createdDate)
    {
        Id = id;
        Title = title;
        Description = description;
        MaxUserCount = maxUserCount;
        CreatorUserId = creatorUserId;
        CreatedDate = createdDate;
    }
}