using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Hubs;
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

    public OffersController(QoqoContext qoqoContext, OfferProvider offerProvider)
    {
        _context = qoqoContext;
        _offerProvider = offerProvider;
    }

    [HttpGet]
    public async Task<List<OfferIndex>> Get()
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
            return ErrorService.BadRequest("Offer not found");
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
}