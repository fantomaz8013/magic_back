using Magic.Domain.Enums;

public static class DiceUtil
{
    private static readonly Random rnd = new();

    public static int RollDice(CubeTypeEnum cubeTypeEnum)
    {
        return cubeTypeEnum switch
        {
            CubeTypeEnum.D4 => rnd.Next(1, 4),
            CubeTypeEnum.D6 => rnd.Next(1, 6),
            CubeTypeEnum.D8 => rnd.Next(1, 8),
            CubeTypeEnum.D10 => rnd.Next(1, 10),
            CubeTypeEnum.D12 => rnd.Next(1, 12),
            CubeTypeEnum.D20 => rnd.Next(1, 20),
            _ => throw new ArgumentOutOfRangeException(nameof(cubeTypeEnum), cubeTypeEnum, null)
        };
    }
}