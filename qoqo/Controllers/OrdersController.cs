using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Providers;
using qoqo.Services;

namespace qoqo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly QoqoContext _context;
    private readonly OrderProvider _orderProvider;

    public OrdersController(QoqoContext qoqoContext, OrderProvider orderProvider)
    {
        _context = qoqoContext;
        _orderProvider = orderProvider;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderViewDto>>> Get()
    {
        return await _orderProvider.GetOrders();
    }

    
    [HttpGet("users/{userId:int}")]
    public async Task<ActionResult<List<OrderViewDto>>> GetFromUser(int userId)
    {
        var tokenService = new TokenService(HttpContext);
        var user = tokenService.GetUser(_context);

        if (user == null)
        {
            return BadRequest();
        }

        return await _orderProvider.GetOrders(user.Id);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderDto>> Get(int id)
    {
        var order = await _orderProvider.GetOrder(id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }
    
    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Patch(int id, OrderBody orderBody)
    {
        return await _orderProvider.UpdateOrder(id, orderBody);
    }
}