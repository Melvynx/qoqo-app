using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Controllers.Admin;

[Route("api/admin/offers")]
[ApiController]
public class AdminOffersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly HubService _hubService;
    private readonly OfferProvider _offerProvider;
    private readonly ITokenService _tokenService;

    public AdminOffersController(QoqoContext qoqoContext, OfferProvider offerProvider, HubService hubService, ITokenService tokenService)
    {
        _context = qoqoContext;
        _offerProvider = offerProvider;
        _hubService = hubService;
        _tokenService = tokenService;
    }
    
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard()
    {
        var dashboard = await _offerProvider.GetDashboard();
        if (dashboard == null) return ErrorService.BadRequest(StringRes.OfferNotFound);

        return dashboard;
    }

    [HttpPut("{id:int}/increase_click")]
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

    [HttpPut("{id:int}/end")]
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
        user = _tokenService.GetUser(HttpContext, _context);

        actionResult = ErrorService.BadRequest(StringRes.NeedToBeLoggedToClick);
        return user is {IsAdmin: true} || true;
    }
}