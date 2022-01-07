namespace qoqo.Model;

public class Token
{
    public int TokenId { get; set; }
    public string Value { get; set; }
    public DateTime? ExpiredAt { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int Id => TokenId;

    public static Token GenerateToken(int userId)
    {
        return new Token
        {
            Value = Guid.NewGuid().ToString(),
            UserId = userId
        };
    }
}