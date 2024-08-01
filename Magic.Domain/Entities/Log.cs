using Magic.Domain.Enums;

namespace Magic.Domain.Entities
{
    /// <summary>
    /// Лог
    /// </summary>
    public class Log : BaseEntity<int>, IHasCreatedDate
    {
        /// <summary>
        /// Уровень
        /// </summary>
        public LogLevelEnum Level { get; set; }
        /// <summary>
        /// Категория
        /// </summary>
        public LogCategoryEnum Category { get; set; }
        /// <summary>
        /// Текст
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}
