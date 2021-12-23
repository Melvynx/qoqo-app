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
}