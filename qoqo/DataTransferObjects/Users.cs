using qoqo.Model;

namespace qoqo.DataTransferObjects;

public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class RegisterDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public int? Npa { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Token { get; set; }
    public bool? IsAdmin { get; set; }
    
    public static UserDto FromUser(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            AvatarUrl = user.AvatarUrl,
            IsAdmin = user.IsAdmin
        };
    }
}

public class UserErrorDto
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    
    public bool IsValid()
    {
        return UserName == null && Password == null && Email == null;
    }
}