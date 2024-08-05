namespace Magic.Domain.Entities
{
    public class GameSessionUser : BaseEntity<int>
    {
        public Guid GameSessionId { get; set; }
        public Guid UserId { get; set; }
        
    }
}
