using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class SetMapRequest
{
    public Guid GameSessionId { get; set; }
    public Guid? MapId { get; set; }
}
public class SetMapRequestValidator : AbstractValidator<SetMapRequest>
{
    public SetMapRequestValidator()
    {
        RuleFor(col => col.GameSessionId).NotNull();
        RuleFor(col => col.MapId);
    }
}