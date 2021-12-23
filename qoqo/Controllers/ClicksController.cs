using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using qoqo.DataTransferObjects;
using qoqo.Hubs;
using qoqo.Model;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClicksController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly IHubContext<OfferHub> _hubContext;

    public ClicksController(QoqoContext qoqoContext, IHubContext<OfferHub> hubContext)
    {
        _context = qoqoContext;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<List<User>> Get()
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", "Hello from the server");

        return _context.Users.ToList();
    }
    
    [HttpGet("offers/{id:int}")]
    public async Task<ActionResult<OfferClickDto>> GetOfferClick(int id)
    {
        var offer = await _context.Offers
            .Select(o => new { Id = o.OfferId, o.ClickObjective })
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null)
        {
            return NotFound();
        }
        
        var clickCount = await _context.Clicks.CountAsync(c => c.OfferId == id);

        return new OfferClickDto { Click = clickCount };
    }

    [HttpPost("offers/{id:int}")]
    public async Task<ActionResult<ClickDto>> AddOfferClick(int id)
    {
        var tokenService = new TokenService(HttpContext);
        var token = tokenService.GetToken();

        if (token == null)
        {
            return BadRequest();
        }

        var userToken = _context.Tokens
            .Include(t => t.User)
            .SingleOrDefault(t => t.Value == token);
        
        if (userToken == null)
        {
            return BadRequest();
        }

        var user = await _context.Users
            .Select(u => new UserClickDto { Id = u.UserId, UserName = u.UserName })
            .FirstOrDefaultAsync(u => u.Id == userToken.UserId);
        
        var offer = await _context.Offers
            .Select(o => new { Id = o.OfferId, o.ClickObjective })
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null || user == null)
        {
            return BadRequest();
        }

        var newClick = new Click
        {
            UserId = userToken.UserId,
            OfferId = id
        };
        
        await _context.Clicks.AddAsync(newClick);
        await _context.SaveChangesAsync();
        var clickCount = await _context.Clicks.CountAsync(c => c.OfferId == id);
        var clickDto = ClickDto.FromUserClick(user, clickCount);

        if (clickCount == offer.ClickObjective)
        {
            Console.WriteLine("Wine!");
        }
        
        await _hubContext.Clients.All.SendAsync("CLICK", JsonService.Serialize(clickDto));

        return Ok();
    }
    
}