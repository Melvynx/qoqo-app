using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Hubs;
using qoqo.Model;

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
    public async Task<ActionResult<OfferClickDto>> GetOffer(int id)
    {
        var offer = await _context.Offers
            .Select(o => new { Id = o.OfferId, o.ClickObjective })
            .FirstOrDefaultAsync(o => o.Id == id);

        if (offer == null)
        {
            return NotFound();
        }
        
        var clickObjective = offer.ClickObjective;
        var clickCount = await _context.Clicks.CountAsync(c => c.OfferId == id);

        return new OfferClickDto { Click = clickObjective - clickCount };
    }
    
}