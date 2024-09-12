using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class ApplyAbilityResponse
{
    public bool IsPossible { get; set; }

    public List<string> Messages { get; set; } = new();

    //id юнитов, что попали под действие способности
    public List<Guid> TargetIds { get; set; } = new();
}