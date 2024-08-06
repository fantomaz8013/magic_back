using Magic.Domain.Entities;

namespace Magic.Common.Models.Response;

public class UserResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserResponse(User user)
    {
        Name = user.Name;
        Login = user.Login;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        Id = user.Id;
    }
}