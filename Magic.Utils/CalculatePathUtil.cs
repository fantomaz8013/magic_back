using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Utils;

public static class CalculatePathUtil
{
    /// <summary>
    /// Проверить, находятся ли точки рядом
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    public static bool IsNeighboringPoint(int x1, int y1, int x2, int y2)
    {
        if (Math.Abs(x1 - x2) > 1)
        {
            return false;
        }
        if (Math.Abs(y1 - y2) > 1)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Получить дистанцию от точки до точки
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <returns></returns>
    public static int Distance(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    /// <summary>
    /// 0,0 1,0 2,0 3,0 4,0 5,0
    /// 0,1 1,1 2,1 3,1 4,1 5,1
    /// 0,2 1,2 2,2 3,2 4,2 5,2
    /// 0,3 1,3 2,3 3,3 4,3 5,3
    /// 0,4 1,4 2,4 3,4 4,4 5,4
    /// 0,5 1,5 2,5 3,5 4,5 5,5
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="radius"></param>
    /// <returns></returns>

    public static List<Point> CalculatedPointsInArea(int x1, int y1, int radius, int mapWidth, int mapHeight)
    {
        var width = mapWidth;
        var height = mapHeight;
        var startX = Math.Max(0, x1 - radius);
        var startY = Math.Max(0, y1 - radius);
        var endX = Math.Min(width - 1, x1 + radius);
        var endY = Math.Min(height - 1, y1 + radius);

        var radiusPoints = new List<Point>();

        for (var y = startY; y <= endY; y++)
        for (var x = startX; x <= endX; x++)
            radiusPoints.Add(new Point(x, y));

        return radiusPoints;
    }
}