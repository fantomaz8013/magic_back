using Magic.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Common.Models.Response
{
    public class PathCalculationResponse
    {
        /// <summary>
        /// Результат просчета пути ( удалось пройти или нет )
        /// </summary>
        public bool IsSuccess { get; set; } = true;
        /// <summary>
        /// Список штрафов при проходе по пути
        /// </summary>
        public List<PenaltyResponse> Penalties { get; set; } = new List<PenaltyResponse>();
        public string Message { get; set; } = "Путь успешно пройден";
        public PositionResponse? NewCharacterPosition { get; set; } = null;
    }


    public class PenaltyResponse
    {
        public TilePropertyPenaltyTypeEnum PenaltyType { get; set; }
        public int Value;
    }

    public class PositionResponse
    {
        public int X {  get; set; }
        public int Y { get; set; }
    }
}
