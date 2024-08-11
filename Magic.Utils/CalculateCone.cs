namespace Magic.Utils;

public class CalculateConeRequest
{
    // например x:5, y:5
    public Point UserPosition { get; set; }

    // например x:6, y:5 => вправо 
    // x:6, y:6 => вправо-вверх диагональ 
    public Point Direction { get; set; }

    //проверки на <=0 нет
    public int Radius { get; set; }
}

public record Point(int X, int Y);

public static class CalculateCone
{
    public static List<Point> GetPointsInCone(CalculateConeRequest request, int mapWidth, int mapHeight)
    {
        var result = new List<Point>();

        var direction = request.Direction;
        var root = request.UserPosition;
        var dx = direction.X - root.X;
        var dy = direction.Y - root.Y;

        var (startX, endX, startY, endY) = GetFigure(dx, dy, request.Radius);
        var remover = GetRemover(dx, dy, root, request.Radius);


        var _startY = Math.Max(startY + root.Y, 0);
        var _endY = Math.Min(endY + root.Y, mapHeight - 1);
        var _startX = Math.Max(startX + root.X, 0);
        var _endX = Math.Min(endX + root.X, mapWidth - 1);

        for (var y = _startY; y <= _endY; y++)
        for (var x = _startX; x <= _endX; x++)
            if (!remover((x, y)))
                result.Add(new Point(x, y));

        return result;
    }

    /// <para>вычисляет координаты фигуры (прямоугольник или квадрат) </para> 
    private static (int startX, int endX, int startY, int endY) GetFigure(
        int dx,
        int dy,
        int radius)
    {
        if (dx != 0 && dy != 0) radius += radius / 2;
        var (endY, startY) = GetMaxMin(dy, radius);
        var (endX, startX) = GetMaxMin(dx, radius);

        return (startX, endX, startY, endY);
    }

    private static (int max, int min) GetMaxMin(int delta, int radius)
    {
        return (
            max: delta >= 0
                ? delta + radius
                : delta,
            min: delta <= 0
                ? delta - radius
                : delta
        );
    }

    /// <para>подбирает фильтрующую функцию </para> 
    /// <para>для квадрата получить половину близжайщую к root </para> 
    /// <para>для прямоугольника получить конус относительно root </para> 
    private static Func<(int x, int y), bool> GetRemover(int dx, int dy, Point root, int radius)
    {
        if (dx == 0 || dy == 0)
            return data =>
            {
                var relativePoint = GetPointRelativeToRoot(data.x, data.y, root);
                var distance = Math.Abs(dx * relativePoint.X + dy * relativePoint.Y);
                return GetDistanceRelativeToRoot(data.x, data.y, root) > GetDistance(distance, distance - 1);
            };

        var distanceToDiagonalInCells = 2 + radius + radius / 2;
        return data =>
        {
            var relativePoint = GetPointRelativeToRoot(data.x, data.y, root);
            return Math.Abs(relativePoint.X) + Math.Abs(relativePoint.Y) > distanceToDiagonalInCells;
        };
    }

    /// Вычисляет растояние от точки до root
    private static double GetDistanceRelativeToRoot(int x, int y, Point root)
    {
        return GetDistance(GetPointRelativeToRoot(x, y, root));
    }

    private static Point GetPointRelativeToRoot(int x, int y, Point root)
    {
        return new Point(root.X - x, root.Y - y);
    }

    private static double GetDistance(Point point)
    {
        return GetDistance(point.X, point.Y);
    }

    private static double GetDistance(int x, int y)
    {
        return Math.Sqrt(x * x + y * y);
    }
}