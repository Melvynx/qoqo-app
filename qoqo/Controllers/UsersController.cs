using Microsoft.AspNetCore.Mvc;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private const string TokenKey = "Token";
    private readonly QoqoContext _context;
    private readonly UserProvider _userProvider;

    public UsersController(QoqoContext qoqoContext)
    {
        _context = qoqoContext;
        _userProvider = new UserProvider(_context);
    }

    [HttpGet]
    public List<User> Get()
    {
        return _context.Users.ToList();
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto?>> Me()
    {
        var token = Request.Cookies["token"];
        if (token == null)
        {
            return BadRequest();
        }

        var user = await _userProvider.GetUserByToken(token);
        if (user == null)
        {
            return BadRequest();
        }

        return user;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto?>> Login(LoginDto loginDto)
    {
        var user = await _userProvider.Login(loginDto);
        if (user == null)
        {
            return NotFound("Invalid username or password");
        }

        SetToken(user.Token);

        return user;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto?>> Register(RegisterDto registerDto)
    {
        var result = await _userProvider.Register(registerDto);
        SetToken(result.Value?.Token);
        return result;
    }

    [HttpPost("logout")]
    public async Task<ActionResult<User?>> Logout()
    {
        var token = GetToken();
        if (token == null)
        {
            return BadRequest();
        }

        Response.Cookies.Delete("Token");

        return await _userProvider.Logout(token) ? Ok("Logout") : BadRequest("Invalid token");
    }

    private void SetToken(string? token)
    {
        if (token == null)
        {
            return;
        }

        Response.Cookies.Append(TokenKey, token);
    }

    private string? GetToken()
    {
        return Request.Cookies[TokenKey];
    }
}