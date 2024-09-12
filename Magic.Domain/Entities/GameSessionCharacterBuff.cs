using Magic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Domain.Entities;

public class GameSessionCharacterBuff : BaseEntity<int>
{
    /// <summary>
    /// Название бафа
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Описание бафа
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Негативный ли эфект от бафа
    /// </summary>
    public bool IsNegative { get; set; }
    /// <summary>
    /// Тип бафа
    /// </summary>
    public BuffTypeEnum BuffType { get; set; }
}