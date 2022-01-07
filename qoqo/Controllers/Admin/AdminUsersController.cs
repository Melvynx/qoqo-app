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
    private readonly UserProvider _userProvider;

    public AdminUsersController(QoqoContext qoqoContext, UserProvider userProvider)
    {
        _context = qoqoContext;
        _userProvider = userProvider;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var isAdminString = GetQueryValue("isAdmin");

        var query = _context.Users.Take(10);
        if (bool.TryParse(isAdminString, out var isAdmin))
        {
            query = query.Where(u => u.IsAdmin == isAdmin);
        }
        
        var userName = GetQueryValue("username");
        if (userName != null)
        {
            query = query.Where(u => u.UserName.ToLower().Contains(userName.ToLower()));
        }

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
    [HttpPatch("{id:int}")]
    public async Task<ActionResult<UserDto>> PatchUser(int id, [FromBody] UserPatchDto userPatch)
    {
        var user = await _userProvider.GetUser(id);
        if (user == null) return ErrorService.BadRequest(StringRes.UserNotFound);
        user.IsAdmin = userPatch.IsAdmin;
        await _context.SaveChangesAsync();
        return SuccessService.Ok(StringRes.UserUpdated);
    }
}