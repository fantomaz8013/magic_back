using FluentValidation;

namespace Magic.Common.Models.Request.GameSessionRequest;

public class CreateGameSessionRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxUserCount { get; set; }
    public DateTime StartDt { get; set; }
}
public class CreateGameSessionRequestValidator : AbstractValidator<CreateGameSessionRequest>
{
    public CreateGameSessionRequestValidator()
    {
        RuleFor(col => col.Title).NotNull();
        RuleFor(col => col.Description).NotNull();
        RuleFor(col => col.MaxUserCount).NotNull();
        RuleFor(col => col.StartDt).NotNull();
    }
}