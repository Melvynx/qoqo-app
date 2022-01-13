using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using qoqo.Model;
using qoqo.Services;

namespace qoqo_test.mock;

public class TokenServiceMock : ITokenService
{
    public void SetToken(HttpContext httpContext, string? token)
    {
    }

    public string? GetToken(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue("Authorization", out var token);
        // Take only the token from "Bearer d584ca25-a6d9-4788-8922-4617d39fa780"
        var tokenString = token.ToString().Split();
        return tokenString.Length > 1 ? tokenString[1] : null;
    }

    public void DeleteToken(HttpContext httpContext)
    {
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