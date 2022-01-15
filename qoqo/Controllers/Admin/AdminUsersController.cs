using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers.Admin;

[Route("api/admin/users")]
[ApiController]
public class AdminUsersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly ITokenService _tokenService;
    private readonly UserProvider _userProvider;

    public AdminUsersController(QoqoContext qoqoContext, UserProvider userProvider, ITokenService tokenService)
    {
        _context = qoqoContext;
        _userProvider = userProvider;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var isAdminString = GetQueryValue("isAdmin");

        var query = _context.Users.Take(10);
        if (bool.TryParse(isAdminString, out var isAdmin)) query = query.Where(u => u.IsAdmin == isAdmin);

        var userName = GetQueryValue("username");
        if (userName != null) query = query.Where(u => u.UserName.ToLower().Contains(userName.ToLower()));

        return await query
            .Select(u => UserDto.FromUser(u, true))
            .ToListAsync();
    }

    private string? GetQueryValue(string key)
    {
        var count = Request.Query[key].Count;
        return count >= 1 ? Request.Query[key][0] : null;
    }

    // controller to patch user only with the isAdmin body
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> PatchUser(int id, [FromBody] UserPatchDto userPatch)
    {
        var currentUser = _tokenService.GetUser(HttpContext, _context);
        if (currentUser == null || currentUser.UserId == id) return Unauthorized();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null) return ErrorService.BadRequest(StringRes.UserNotFound);

        user.IsAdmin = userPatch.IsAdmin;
        await _context.SaveChangesAsync();
        return SuccessService.Ok(StringRes.UserUpdated);
    }
}