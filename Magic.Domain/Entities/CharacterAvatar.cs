using Magic.Domain.Enums;

namespace Magic.Domain.Entities
{
    public class CharacterAvatar : BaseEntity<int>
    {
        /// <summary>
        /// Название города
        /// </summary>
        public string AvatarUrl { get; set; }
    }
}
