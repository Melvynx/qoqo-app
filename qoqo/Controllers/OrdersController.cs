using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Services;

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
    
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> Get()
    {
        var tokenService = new TokenService(HttpContext);
        var user = tokenService.GetUser(_context);

        if (user == null)
        {
            return BadRequest();
        }

        var orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .Include(o => o.Click)
            .Include(o => o.Click.Offer)
            .Select(o => new OrderDto
            {
                Offer = new OfferOrderDto
                {
                    Title = o.Click.Offer.Title,
                    Id = o.Click.Offer.Id
                },
                CreatedAt = o.CreatedAt,
                Status = o.Status
            }).ToListAsync();
        return orders;
    }
}