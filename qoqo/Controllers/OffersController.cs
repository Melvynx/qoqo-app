using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
}