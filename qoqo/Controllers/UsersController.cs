using Microsoft.AspNetCore.Mvc;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
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

    [HttpPost("login")]
    public async Task<ActionResult<User?>> Login(LoginDto loginDto)
    {
        var user = await _userProvider.Login(loginDto);
        if (user == null)
        {
            return NotFound("Invalid username or password");
        }

        return user;
    }
}