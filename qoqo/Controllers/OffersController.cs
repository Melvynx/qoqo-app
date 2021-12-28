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
    public async Task<ActionResult<Offer>> GetOffers()
    {
        return Ok("Todo");
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