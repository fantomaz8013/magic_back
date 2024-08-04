using Magic.Domain.Entities;

namespace Magic.DAL.Dto.Implementations;

public class UserDto : IDtoConfiguration<User>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int RoleId { get; set; }

    public UserDto() { }
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
    }

    public void Configure(DtoBuilder<User> builder)
    {
        builder.AddProperty(x => x.Id);
        builder.AddProperty(x => x.Name)
            .AddSearch(x => x.Name);
        builder.AddProperty(x => x.Email)
            .AddSearch(x => x.Email);
        builder.AddProperty(x => x.PhoneNumber)
            .AddSearch(x => x.PhoneNumber);
    }
}