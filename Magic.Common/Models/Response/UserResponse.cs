namespace Magic.Common.Models.Response;

public class UserResponse
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public UserResponse(string name, string login, string email, string phoneNumber, Guid id)
    {
        Name = name;
        Login = login;
        Email = email;
        PhoneNumber = phoneNumber;
        Id = id;
    }
}