using FluentValidation;

namespace Magic.Common.Models.Request
{
    public class TokenRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class TokenRequestValidator : AbstractValidator<TokenRequest>
    {
        public TokenRequestValidator()
        {
            RuleFor(col => col.Login).NotNull();
            RuleFor(col => col.Password).NotNull();
        }
    }
}
