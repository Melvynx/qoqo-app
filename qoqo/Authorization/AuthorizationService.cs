using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using qoqo.Model;
using qoqo.Services;

namespace qoqo.Authorization;

public class AuthorizationService : IAuthenticationService
{
    private QoqoContext _qoqoContext;
    
    public AuthorizationService(QoqoContext qoqoContext)
    {
        _qoqoContext = qoqoContext;
    }
    
    public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string scheme)
    {
        throw new NotImplementedException();
    }

    public Task ChallengeAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        var tokenService = new TokenService(context);
        var token = tokenService.GetToken();

        if (token == null)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }

        var userToken = _qoqoContext.Tokens
            .Include(t => t.User)
            .SingleOrDefault(t => t.Value == token);

        if (userToken?.User == null)
        {
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            return Task.CompletedTask;
        }
        
        var claims = new List<Claim>
        {
            new("UserId", userToken.User.UserId.ToString()),
        };
        
        // get with: context.User.Claims.First(d => d.Type == "Id").Value;
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, scheme));
        
        return Task.FromResult(0);
    }

    public Task ForbidAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(HttpContext context, string scheme, ClaimsPrincipal principal,
        AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }

    public Task SignOutAsync(HttpContext context, string scheme, AuthenticationProperties properties)
    {
        throw new NotImplementedException();
    }
}