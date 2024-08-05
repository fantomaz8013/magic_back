using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class DeleteGameSessionRequest
{
    public Guid GameSessionId { get; set; }
}
public class DeleteGameSessionRequestValidator : AbstractValidator<DeleteGameSessionRequest>
{
    public DeleteGameSessionRequestValidator()
    {
        RuleFor(col => col.GameSessionId).NotNull();
    }
}