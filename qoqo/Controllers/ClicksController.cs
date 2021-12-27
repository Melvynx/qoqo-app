using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using qoqo.DataTransferObjects;
using qoqo.Hubs;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClicksController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly IHubContext<OfferHub> _hubContext;
    private readonly ClickProvider _clickProvider;

    public ClicksController(QoqoContext qoqoContext, IHubContext<OfferHub> hubContext, ClickProvider clickProvider)
    {
        _context = qoqoContext;
        _hubContext = hubContext;
        _clickProvider = clickProvider;
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
            .Select(o => new {Id = o.OfferId, o.ClickObjective})
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null)
        {
            return NotFound();
        }

        var clickCount = await _clickProvider.GetCountForOffer(id);

        var tokenService = new TokenService(HttpContext);
        var user = tokenService.GetUser(_context);

        if (user == null)
        {
            return new OfferClickDto {Click = clickCount};
        }

        var lastClick = await _context.Clicks
            .Where(c => c.OfferId == id && c.UserId == user.UserId)
            .OrderByDescending(c => c.ClickId)
            .FirstOrDefaultAsync();

        if (lastClick == null)
        {
            return new OfferClickDto {Click = clickCount};
        }

        var totalSeconds = DateTime.Now.Subtract(lastClick.CreatedAt).TotalSeconds;
        // if lastClick was created less than 10 seconds ago then return BadRequest
        return new OfferClickDto
        {
            Click = clickCount, 
            RemainingTime = Convert.ToInt32(Math.Round(Math.Min(Math.Max(10 - totalSeconds, 0),10)))
        };
    }

    [HttpPost("offers/{id:int}")]
    public async Task<ActionResult<ClickDto>> AddOfferClick(int id)
    {
        var tokenService = new TokenService(HttpContext);
        var user = tokenService.GetUser(_context);

        if (user == null)
        {
            return ErrorService.BadRequest(StringRes.NeedToBeLoggedToClick);
        }

        var userDto = UserClickDto.FromUser(user);
        var offer = await _context.Offers
            .Select(o => new {Id = o.OfferId, o.ClickObjective})
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null)
        {
            return ErrorService.BadRequest(StringRes.OfferNotFound);
        }

        var lastClick = await _context.Clicks
            .Where(c => c.OfferId == id && c.UserId == user.UserId)
            .OrderByDescending(c => c.ClickId)
            .FirstOrDefaultAsync();

        // if lastClick was created less than 10 seconds ago then return BadRequest
        if (lastClick != null && DateTime.Now.Subtract(lastClick.CreatedAt).TotalSeconds < 10)
        {
            return ErrorService.BadRequest(StringRes.ClickMinimum10Seconds);
        }            

        await _clickProvider.Add(user.UserId, id);
        var clickCount = await _clickProvider.GetCountForOffer(id);
        var clickDto = ClickDto.FromUserClick(userDto, clickCount);

        if (clickCount == offer.ClickObjective)
        {
            // TODO: Handle victory
            Console.WriteLine("Wine!");
        }

        await _hubContext.Clients.All.SendAsync("CLICK", JsonService.Serialize(clickDto));

        return Ok(clickDto);
    }
}