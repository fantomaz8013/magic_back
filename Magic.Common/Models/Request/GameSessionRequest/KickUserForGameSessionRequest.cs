using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class KickUserForGameSessionRequest
{
    public Guid UserId { get; set; }
    public Guid GameSessionId { get; set; }
}
public class KickUserForGameSessionRequestValidator : AbstractValidator<KickUserForGameSessionRequest>
{
    public KickUserForGameSessionRequestValidator()
    {
        RuleFor(col => col.UserId).NotNull();
        RuleFor(col => col.GameSessionId).NotNull();
    }
}