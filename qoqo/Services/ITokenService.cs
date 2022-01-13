using qoqo.Model;

namespace qoqo.Services;

public interface ITokenService
{
    void SetToken(HttpContext httpContext, string? token);
    string? GetToken(HttpContext httpContext);
    void DeleteToken(HttpContext httpContext);
    User? GetUser(HttpContext httpContext, QoqoContext context);
}