using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class LeaveGameSessionRequest
{
    public Guid GameSessionId { get; set; }
}
public class LeaveGameSessionRequestValidator : AbstractValidator<LeaveGameSessionRequest>
{
    public LeaveGameSessionRequestValidator()
    {
        RuleFor(col => col.GameSessionId).NotNull();
    }
}