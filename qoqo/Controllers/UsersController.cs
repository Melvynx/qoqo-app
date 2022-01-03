using System.Net;
using Microsoft.AspNetCore.Mvc;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly UserProvider _userProvider;

    public UsersController(QoqoContext qoqoContext, UserProvider userProvider)
    {
        _context = qoqoContext;
        _userProvider = userProvider;
    }

    [HttpGet]
    public List<User> Get()
    {
        return _context.Users.ToList();
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UserDto>> Patch(int id, [FromBody] UserDto user)
    {
        return await _userProvider.UpdateUser(user, id);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto?>> Me()
    {
        var user = new TokenService(HttpContext).GetUser(_context);
        return user == null ? ErrorService.BadRequest("Invalid Token") : UserDto.FromUser(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto?>> Login(LoginDto loginDto)
    {
        var user = await _userProvider.Login(loginDto);
        if (user == null)
        {
            return NotFound(StringRes.LoginFailed);
        }

        new TokenService(HttpContext).SetToken(user.Token);

        return user;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto?>> Register(RegisterDto registerDto)
    {
        var result = await _userProvider.Register(registerDto);
        new TokenService(HttpContext).SetToken(result.Value?.Token);
        return result;
    }

    [HttpPost("logout")]
    public async Task<ActionResult<User?>> Logout()
    {
        var tokenService = new TokenService(HttpContext);
        var token = tokenService.GetToken();

        if (token == null)
        {
            return BadRequest("No token found");
        }

        tokenService.DeleteToken();

        return await _userProvider.Logout(token)
            ? SuccessService.Ok(StringRes.Logout)
            : ErrorService.BadRequest(StringRes.LogoutFailed);
    }
}