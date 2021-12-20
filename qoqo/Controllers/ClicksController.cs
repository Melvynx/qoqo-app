using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
}