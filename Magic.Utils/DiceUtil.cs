using Magic.Domain.Enums;

namespace Magic.Utils;

public static class DiceUtil
{
    private static readonly Random rnd = new();
    private const int MaxBoundIncluder = 1;

    public static int RollDice(CubeTypeEnum cubeTypeEnum)
    {
        return cubeTypeEnum switch
        {
            CubeTypeEnum.D4 => rnd.Next(1, 4 + MaxBoundIncluder),
            CubeTypeEnum.D6 => rnd.Next(1, 6 + MaxBoundIncluder),
            CubeTypeEnum.D8 => rnd.Next(1, 8 + MaxBoundIncluder),
            CubeTypeEnum.D10 => rnd.Next(1, 10 + MaxBoundIncluder),
            CubeTypeEnum.D12 => rnd.Next(1, 12 + MaxBoundIncluder),
            CubeTypeEnum.D20 => rnd.Next(1, 20 + MaxBoundIncluder),
            _ => throw new ArgumentOutOfRangeException(nameof(cubeTypeEnum), cubeTypeEnum, null)
        };
    }
}