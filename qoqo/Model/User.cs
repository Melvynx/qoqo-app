namespace qoqo.Model;

public class User
{
    public int UserId { get; set; }
    public string Usernamne { get; set; }
    public string PasswordHash { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    
    public int Id => UserId;
}