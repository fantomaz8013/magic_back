using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class EnterToGameSessionRequest
{
    public Guid GameSessionId { get; set; }
}
public class EnterToGameSessionRequestValidator : AbstractValidator<EnterToGameSessionRequest>
{
    public EnterToGameSessionRequestValidator()
    {
        RuleFor(col => col.GameSessionId).NotNull();
    }
}