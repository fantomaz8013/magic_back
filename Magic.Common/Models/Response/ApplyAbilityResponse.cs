using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class ApplyAbilityResponse
{
    public bool IsResult { get; set; }
    public List<string> Message { get; set; } = new();
    public List<Guid> TargetIds { get; set; } = new();
}