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
    [HttpGet("{id:int}")]
    public async Task<OfferDto?> Get(int id)
    {
        return await _offerProvider.GetOffer(id);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, [FromBody] OfferBody offer)
    {
        return await _offerProvider.UpdateOffer(id, offer);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] OfferBody offer)
    {
        var createdOffer = await _offerProvider.CreateOffer(offer);
        return createdOffer == null
            ? ErrorService.BadRequest(StringRes.ErrorDuringOfferCreation)
            : SuccessService.Ok(StringRes.OfferCreated);
    }

    [HttpGet("current")]
    public async Task<ActionResult<Offer>> GetCurrentOffer()
    {
        var offer = await _offerProvider.GetCurrentOffer();
        if (offer == null)
        {
            return ErrorService.BadRequest(StringRes.OfferNotFound);
        }

        return offer;
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