using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Magic.Common.Models.Request
{
    public class UserUpdateRequest
    {
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Description { get; set; }
        public string? GameExperience { get; set; }
        public IFormFile? Avatar { get; set; }
        public int? CityId { get; set; }
    }
    public class UserUpdateRequestValidator : AbstractValidator<UserUpdateRequest>
    {
        private const string _phoneNumber = @"^([\+]?7[-]?|[8])?[1-9][0-9]{9}$";
        private const string _email = @"^[\w\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public UserUpdateRequestValidator()
        {
            RuleFor(col => col.Name);
            RuleFor(col => col.Description);
            RuleFor(col => col.GameExperience);
            RuleFor(col => col.PhoneNumber).Matches(_phoneNumber);
            RuleFor(col => col.Email).Matches(_email);
            RuleFor(col => col.Avatar)
                .SetValidator(new AvatarValidator());
            RuleFor(col => col.CityId);
        }
    }

    public class AvatarValidator : AbstractValidator<IFormFile?>
    {
        public AvatarValidator()
        {
            RuleFor(x => x.ContentType).Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"));
        }
    }
}
