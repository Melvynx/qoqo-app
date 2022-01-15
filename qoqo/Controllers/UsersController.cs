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
    private readonly ITokenService _tokenService;
    private readonly UserProvider _userProvider;

    public UsersController(QoqoContext qoqoContext, UserProvider userProvider, ITokenService tokenService)
    {
        _context = qoqoContext;
        _userProvider = userProvider;
        _tokenService = tokenService;
    }

    [HttpGet]
    public List<User> Get()
    {
        return _context.Users.ToList();
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Patch(int id, [FromBody] UserDto user)
    {
        var currentUser = _tokenService.GetUser(HttpContext, _context);

        return currentUser?.UserId == id ? await _userProvider.UpdateUser(user, id) : BadRequest();
    }

    [HttpGet("me")]
    public ActionResult<UserDto?> Me()
    {
        var user = _tokenService.GetUser(HttpContext, _context);
        return user == null ? ErrorService.BadRequest("Invalid Token") : UserDto.FromUser(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto?>> Login(LoginDto loginDto)
    {
        var user = await _userProvider.Login(loginDto);
        if (user == null) return NotFound(StringRes.LoginFailed);

        _tokenService.SetToken(HttpContext, user.Token);

        return user;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto?>> Register(RegisterDto registerDto)
    {
        var result = await _userProvider.Register(registerDto);
        _tokenService.SetToken(HttpContext, result.Value?.Token);
        return result;
    }

    [HttpPost("logout")]
    public async Task<ActionResult<User?>> Logout()
    {
        var token = _tokenService.GetToken(HttpContext);

        if (token == null) return BadRequest("No token found");

        _tokenService.DeleteToken(HttpContext);

        return await _userProvider.Logout(token)
            ? SuccessService.Ok(StringRes.Logout)
            : ErrorService.BadRequest(StringRes.LogoutFailed);
    }
}