using System.Text.RegularExpressions;

namespace qoqo.Services;

public static class RegexService
{
    
    private static readonly Regex PasswordRegex = new("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.{8,})");
    // private static readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,1000}$");
    private static readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_]{3,30}$");
    private static readonly Regex EmailRegex = new(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$");

    public static bool CheckPassword(string password)
    {
        return PasswordRegex.IsMatch(password);
    }
    
    public static bool CheckUserName(string username)
    {
        return UsernameRegex.IsMatch(username);
    }
    
    public static bool CheckEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}