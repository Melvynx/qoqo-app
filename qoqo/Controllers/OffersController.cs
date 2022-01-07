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
public class OffersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly OfferProvider _offerProvider;
    private readonly HubService _hubService;

    public OffersController(QoqoContext qoqoContext, OfferProvider offerProvider, HubService hubService)
    {
        _context = qoqoContext;
        _offerProvider = offerProvider;
        _hubService = hubService;
    }

    [HttpGet]
    public async Task<List<OfferIndexDto>> Get()
    {
        return await _offerProvider.GetOffers();
    }

    // TODO: Add authorization
    [HttpGet("{id}")]
    public async Task<ActionResult<OfferDto?>> Get(string id)
    {
        if (id == "current")
        {
            var currentOffer = await _offerProvider.GetCurrentOffer();
            return currentOffer == null ? BadRequest() : currentOffer;
        }

        if (!int.TryParse(id, out var offerId)) return BadRequest();

        var tokenProvider = new TokenService(HttpContext);
        var user = tokenProvider.GetUser(_context);
        var offer = await _offerProvider.GetOffer(offerId);

        if (offer == null)
        {
            return ErrorService.BadRequest(StringRes.OfferNotFound);
        }

        if (user is {IsAdmin: false} && (offer.IsDraft || offer.StartAt < DateTime.Now))
        {
            return Unauthorized();
        }

        return offer;
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] OfferBody offer)
    {
        return await _offerProvider.UpdateOffer(id, offer);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] OfferBody offer)
    {
        return await _offerProvider.CreateOffer(offer);
    }

    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var dashboard = await _offerProvider.GetDashboard();
        if (dashboard == null)
        {
            return ErrorService.BadRequest(StringRes.OfferNotFound);
        }

        return dashboard;
    }

    [HttpPatch("{id:int}/increase_click")]
    public async Task<ActionResult> IncreaseClick(int id)
    {
        if (!IsAdminUser(out var action, out var user))
            return action;

        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.OfferId == id);
        if (offer == null) return ErrorService.BadRequest(StringRes.OfferNotFound);
        offer.ClickObjective++;
        await _context.SaveChangesAsync();

        var clickDto = ClickDto.FromUserClick(UserClickDto.FromUser(user), 0, offer.ClickObjective);

        await _hubService.Click(clickDto);
        return SuccessService.Ok(StringRes.OfferUpdated);
    }

    [HttpPatch("{id:int}/end")]
    public async Task<ActionResult> End(int id)
    {
        if (!IsAdminUser(out var action, out _))
            return action;

        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.OfferId == id);
        if (offer == null) return ErrorService.BadRequest(StringRes.OfferNotFound);
        offer.EndAt = DateTime.Now.AddDays(-1);
        offer.IsDraft = true;
        await _context.SaveChangesAsync();
        return SuccessService.Ok(StringRes.OfferUpdated);
    }

    private bool IsAdminUser(out ActionResult actionResult, out User? user)
    {
        var tokenService = new TokenService(HttpContext);
        user = tokenService.GetUser(_context);

        actionResult = ErrorService.BadRequest(StringRes.NeedToBeLoggedToClick);
        return user is {IsAdmin: true} || true;
    }
}