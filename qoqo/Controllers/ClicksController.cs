using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClicksController : ControllerBase
{
    private readonly ClickProvider _clickProvider;
    private readonly QoqoContext _context;
    private readonly HubService _hubContext;
    private readonly ITokenService _tokenService;

    public ClicksController(QoqoContext qoqoContext, HubService hubService, ClickProvider clickProvider, ITokenService tokenService)
    {
        _context = qoqoContext;
        _hubContext = hubService;
        _clickProvider = clickProvider;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserClick>>> Get()
    {
        var user = _tokenService.GetUser(HttpContext, _context);

        if (user == null) return BadRequest();

        var clicks = await _context.Clicks
            .Include(c => c.Offer)
            .Where(c => c.UserId == user.UserId)
            .GroupBy(c => new {c.Offer.Title, c.OfferId})
            .Select(g => new UserClick
            {
                Count = g.Count(),
                OfferTitle = g.Key.Title,
                OfferId = g.Key.OfferId
            })
            .ToListAsync();
        return clicks;
    }

    [HttpGet("offers/{id:int}")]
    public async Task<ActionResult<OfferClickDto>> GetOfferClick(int id)
    {
        var offer = await _context.Offers
            .Select(o => new {Id = o.OfferId, o.ClickObjective, WinnerSentence = o.WinnerText})
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null) return NotFound();

        var clickCount = await _clickProvider.GetCountForOffer(id);

        var user = _tokenService.GetUser(HttpContext, _context);

        if (user == null) return new OfferClickDto {Click = clickCount};

        var lastClick = await _context.Clicks
            .Where(c => c.OfferId == id && c.UserId == user.UserId)
            .OrderByDescending(c => c.ClickId)
            .FirstOrDefaultAsync();

        if (lastClick == null) return new OfferClickDto {Click = clickCount};

        var totalSeconds = DateTime.Now.Subtract(lastClick.CreatedAt).TotalSeconds;
        // if lastClick was created less than 10 seconds ago then return BadRequest
        return new OfferClickDto
        {
            Click = clickCount,
            RemainingTime = Convert.ToInt32(Math.Round(Math.Min(Math.Max(10 - totalSeconds, 0), 10)))
        };
    }

    [HttpPost("offers/{id:int}")]
    public async Task<ActionResult<ClickDto>> AddOfferClick(int id)
    {
        var user = _tokenService.GetUser(HttpContext, _context);
        
        if (user == null) return ErrorService.Unauthorized(StringRes.NeedToBeLoggedToClick);

        var userDto = UserClickDto.FromUser(user);
        var offer = await _context.Offers
            .Select(o => new {Id = o.OfferId, o.ClickObjective, o.IsDraft, o.StartAt, o.EndAt})
            .FirstOrDefaultAsync(o => o.Id == id);

        var now = DateTime.Now;
        if (offer is not {IsDraft: false} || offer.StartAt > now || offer.EndAt < now)
            return ErrorService.BadRequest(StringRes.OfferNotFound);

        var lastClick = await _context.Clicks
            .Where(c => c.OfferId == id && c.UserId == user.UserId)
            .OrderByDescending(c => c.ClickId)
            .FirstOrDefaultAsync();

        // if lastClick was created less than 10 seconds ago then return BadRequest
        if (lastClick != null && DateTime.Now.Subtract(lastClick.CreatedAt).TotalSeconds < 10)
            return ErrorService.BadRequest(StringRes.ClickMinimum10Seconds);

        var clickCount = await _clickProvider.GetCountForOffer(id);

        if (clickCount == offer.ClickObjective)
            return ErrorService.BadRequest(StringRes.OfferClickEnoughTime);

        var click = await _clickProvider.Add(user.UserId, id);
        clickCount += 1;

        var clickDto = ClickDto.FromUserClick(userDto, clickCount, offer.ClickObjective);
        try
        {
            return Ok(clickCount == offer.ClickObjective
                ? await _hubContext.Finish(clickDto, click)
                : await _hubContext.Click(clickDto));
        }
        catch (DbUpdateException)
        {
            return ErrorService.BadRequest("An error is occurred while saving order to database");
        }
    }
}