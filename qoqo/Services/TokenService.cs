using Microsoft.EntityFrameworkCore;
using qoqo.Model;

namespace qoqo.Services;

public class TokenService
{
    private readonly HttpContext _httpContext;
    
    public TokenService(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }
    
    private const string TokenKey = "Token";

    public void SetToken(string? token)
    {
        if (token == null)
        {
            return;
        }

        _httpContext.Response.Cookies.Append(TokenKey, token);
    }

    public string? GetToken()
    {
        return _httpContext.Request.Cookies[TokenKey];
    }

    public void DeleteToken()
    {
        _httpContext.Response.Cookies.Delete(TokenKey);
    }

    public User? GetUser(QoqoContext context)
    {
        var token = GetToken();

        if (token == null)
        {
            return null;
        }

        var userToken = context.Tokens
            .Include(t => t.User)
            .SingleOrDefault(t => t.ExpiredAt == null && t.Value == token);

        return userToken?.User;
    }
}