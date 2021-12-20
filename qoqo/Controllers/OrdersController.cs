using Microsoft.AspNetCore.Mvc;
using qoqo.Model;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly QoqoContext _context;

    public OrdersController(QoqoContext qoqoContext)
    {
        _context = qoqoContext;
    }
}