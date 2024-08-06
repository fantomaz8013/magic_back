namespace Magic.Domain.Entities;

/// <summary>
/// Игровая комната
/// </summary>
public class GameSession : BaseEntity<Guid>, IHasCreatedDate
{
    /// <summary>
    /// Название игровой комнаты
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Описание игровой комнаты
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Максимальное количество игроков в комнате ( пользователь создавший комнату в учет не идет )
    /// </summary>
    public int MaxUserCount { get; set; }
    /// <summary>
    /// Идентификатор пользователя создавшего комнату ( он же мастер )
    /// </summary>
    public Guid CreatorUserId { get; set; }
    /// <summary>
    /// Пользователь создавший комнату
    /// </summary>
    public User CreatorUser { get; set; }
    /// <summary>
    /// Пользователи в комнате
    /// </summary>
    public List<User> Users { get; set; } = new ();
    /// <summary>
    /// Дата старта игровой сессии ( когда и во сколько начинается игра ) 
    /// </summary>
    public DateTime StartDt { get; set; }
    /// <summary>
    /// Дата создания игровой сессии
    /// </summary>
    public DateTime CreatedDate { get; set; }
}