namespace qoqo.Model;

public class User
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsAdmin { get; set; } = false;
    public string? Street { get; set; }
    public int? Npa { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public List<Click> Clicks { get; set; }
    public List<Order> Orders { get; set; }
    public List<Token> Tokens { get; set; }

    public string? GetFullAddress()
    {
        if (Street == null || Npa == null || City == null)
        {
            return null;
        }
        return $"{Street} {Npa} {City}";
    }
}