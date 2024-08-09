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

public class CalculateCone
{
    public static List<Point> GetPointsInCone(CalculateConeRequest request, int mapWidth, int mapHeight)
    {
        var result = new List<Point>();

        var direction = request.Direction;
        var root = request.UserPosition;
        var dx = direction.X - root.X;
        var dy = direction.Y - root.Y;

        var (startX, endX, startY, endY, maxValue, minValue) =
            GetFigure(dx, dy, request.Radius, root, mapWidth, mapHeight);
        var remover = GetRemover(dx, dy, maxValue, minValue, root);

        for (var y = startY; y <= endY; y++)
        for (var x = startX; x <= endX; x++)
            if (!remover((x, y)))
                result.Add(new Point(x, y));

        return result;
    }

    /// <para>вычисляет координаты фигуры (прямоугольник или квадрат) </para> 
    /// <para>maxValue - наибольшая координата (в абсолютном значениии) </para> 
    /// <para>min - наименьшая </para> 
    private static (int startX, int endX, int startY, int endY, int maxValue, int minValue) GetFigure(
        int dx,
        int dy,
        int radius,
        Point root,
        int mapWidth,
        int mapHeight)
    {
        var (maxY, minY) = GetMaxMin(dy, radius);
        var (maxX, minX) = GetMaxMin(dx, radius);
        var startY = Math.Max(minY + root.Y, 0);
        var endY = Math.Min(maxY + root.Y, mapHeight - 1);
        var startX = Math.Max(minX + root.X, 0);
        var endX = Math.Min(maxX + root.X, mapWidth - 1);
        var values = new[] { startX, endX, startY, endY }.Select(Math.Abs).ToArray();
        var maxValue = values.Max();
        var minValue = values.Min();

        return (startX, endX, startY, endY, maxValue, minValue);
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
    private static Func<(int x, int y), bool> GetRemover(int dx, int dy, int max, int min, Point root)
    {
        if (dx == 0 || dy == 0)
            return data =>
            {
                var d = Math.Abs(dx * data.x + dy * data.y);
                return GetDistanceRelativeToRoot(data.x, data.y, root) > GetDistanceRelativeToRoot(d, d - 1, root);
            };

        var d = GetDistanceRelativeToRoot(max, min, root);
        return data => GetDistanceRelativeToRoot(data.x, data.y, root) > d;
    }

    /// Вычисляет растояние от точки до root
    private static double GetDistanceRelativeToRoot(int x, int y, Point root)
    {
        var dx = root.X - x;
        var dy = root.Y - y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}