using Microsoft.EntityFrameworkCore;
using qoqo.Model;

namespace qoqo.Services;

public class TokenService : ITokenService
{
    private const string TokenKey = "Token";

    public void SetToken(HttpContext httpContext, string? token)
    {
        if (token == null) return;

        var now = DateTime.Now;

        var option = new CookieOptions
        {
            Expires = now.AddMonths(3)
        };

        httpContext.Response.Cookies.Append(TokenKey, token, option);
    }

    public string? GetToken(HttpContext httpContext)
    {
        return httpContext.Request.Cookies[TokenKey];
    }

    public void DeleteToken(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(TokenKey);
    }

    public User? GetUser(HttpContext httpContext, QoqoContext context)
    {
        var token = GetToken(httpContext);

        if (token == null) return null;

        var userToken = context.Tokens
            .Include(t => t.User)
            .SingleOrDefault(t => t.ExpiredAt == null && t.Value == token);

        return userToken?.User;
    }
}