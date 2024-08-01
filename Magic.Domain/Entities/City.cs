using Magic.Domain.Enums;

namespace Magic.Domain.Entities
{
    public class City : BaseEntity<int>
    {
        /// <summary>
        /// Название города
        /// </summary>
        public string Title { get; set; }
    }
}
