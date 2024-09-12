using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class TurnInfoResponse
{
    public int LeftStep { get; set; }
    public int SkipStepCount { get; set; }
    public int LeftMainAction { get; set; }
    public int LeftBonusAction { get; set; }
    public Guid GameSessionCharacterId { get; set; }
    public List<AbilityCoolDowns> AbilityCoolDowns { get; set; }
    public List<BuffCoolDowns> BuffCoolDowns { get; set; }


    public TurnInfoResponse(GameSessionCharacterTurnInfo turnInfo)
    {
        LeftStep = turnInfo.LeftStep;
        SkipStepCount = turnInfo.SkipStepCount;
        LeftMainAction = turnInfo.LeftMainAction;
        LeftBonusAction = turnInfo.LeftBonusAction;
        GameSessionCharacterId = turnInfo.GameSessionCharacterId;
        AbilityCoolDowns = turnInfo.AbilityCoolDowns;
        BuffCoolDowns = turnInfo.BuffCoolDowns;
    }
}