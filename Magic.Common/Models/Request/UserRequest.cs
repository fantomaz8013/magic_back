using FluentValidation;

namespace Magic.Common.Models.Request
{
    public class UserRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        private const string _phoneNumber = @"^([\+]?7[-]?|[8])?[1-9][0-9]{9}$";
        private const string _email = @"^[\w\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public UserRequestValidator()
        {
            RuleFor(col => col.Login).NotNull();
            RuleFor(col => col.Password).NotNull();
        }
    }
}
