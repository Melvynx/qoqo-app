using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using qoqo.Hubs;
using qoqo.Model;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OffersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly IHubContext<OfferHub> _hubContext;

    public OffersController(QoqoContext qoqoContext, IHubContext<OfferHub> hubContext)
    {
        _context = qoqoContext;
        _hubContext = hubContext;
    }
    
    [HttpGet]
    public async Task<ActionResult<Offer>> GetOffers()
    {
        return Ok("Todo");
    }
    
    [HttpGet("current")]
    public async Task<ActionResult<Offer>> GetCurrentOffer()
    {
        var today = DateTime.Today;
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.StartAt <= today && o.EndAt >= today);
        if (offer == null)
        {
            return NotFound("No current offer");
        }

        return offer;
    }
}