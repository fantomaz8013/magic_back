using Magic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain.Entities
{
    public class TileProperty : BaseEntity<int>
    {
        /// <summary>
        /// Название тайла
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Описание тайла
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Изображение тайла
        /// </summary>
        public string Image {  get; set; }
        /// <summary>
        /// Тип колизии тайла
        /// </summary>
        public TilePropertyCollisionTypeEnum CollisionType { get; set; }
        /// <summary>
        /// Тип штрафа за проход по тайлу
        /// </summary>
        public TilePropertyPenaltyTypeEnum PenaltyType { get; set; }
        /// <summary>
        /// Тип возможного действия по отношению к тайлу
        /// </summary>
        public TilePropertyTargetTypeEnum TargetType { get; set; }
        /// <summary>
        /// Количество прочности у тайла
        /// </summary>
        public int? Health { get; set; }
        /// <summary>
        /// Идентификатор тайла, который должен появится при разрушении
        /// </summary>
        public int? TilePropertyIdIfDestroyed { get; set; }
        /// <summary>
        /// Тайл, который должен появится при разрушении
        /// </summary>
        public TileProperty TilePropertyIfDestroyed { get; set; }
        /// <summary>
        /// Количество штрафа за проход по клетке
        /// </summary>
        public int? PenaltyValue { get; set; }
    }
}
