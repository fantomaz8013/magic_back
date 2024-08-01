namespace Magic.Domain.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User : BaseEntity<Guid>, IHasBlocked, IHasCreatedDate
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// О себе
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Путь к аватару
        /// </summary>
        public string? AvatarUrl { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string? PhoneNumber { get; set; }
        /// <summary>
        /// Опыт игры в D&D
        /// </summary>
        public string? GameExperience { get; set; }
        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string PasswordHash { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PasswordSalt { get; set; }
        /// <summary>
        /// Реферальный код
        /// </summary>
        public string? RefKey { get; set; }
        /// <summary>
        /// Идентификатор пользователя инициатора
        /// </summary>
        public Guid? RefUserId { get; set; }
        /// <summary>
        /// Пользователь инициатор
        /// </summary>
        public User? RefUser { get; set; }
        /// <summary>
        /// Статус блокировки
        /// </summary>
        public bool IsBlocked { get; set; }
        /// <summary>
        /// Дата блокировки
        /// </summary>
        public DateTime? BlockedDate { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Идентификатор города
        /// </summary>
        public int? CityId { get; set; }
        /// <summary>
        /// Город
        /// </summary>
        public City? City { get; set; }
    }
}
