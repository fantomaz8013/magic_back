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
        public string Name { get; set; }
        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public string PhoneNumber { get; set; }
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
        /// Код для авторизация
        /// </summary>
        public string? Code { get; set; }
        /// <summary>
        /// Дата кода авторизация
        /// </summary>
        public DateTime? CodeDate { get; set; }
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
    }
}
