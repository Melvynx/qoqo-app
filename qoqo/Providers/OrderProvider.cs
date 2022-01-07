using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;
using qoqo.Ressources;
using qoqo.Services;

namespace qoqo.Providers;

public class OrderProvider
{
    private readonly QoqoContext _context;

    public OrderProvider(QoqoContext context)
    {
        _context = context;
    }

    // get the order
    public async Task<OrderDto?> GetOrder(int id)
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Offer)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                Status = o.Status,
                user = UserDto.FromUser(o.User, true),
                offer = new OfferOrderDto
                {
                    OfferId = o.OfferId,
                    Title = o.Offer.Title
                }
            }).SingleOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<List<OrderViewDto>> GetOrders(int? userId = null)
    {
        return await _context.Orders
            .Where(o => userId == null || o.UserId == userId)
            .Include(o => o.Offer)
            .Select(o => new OrderViewDto
            {
                OrderId = o.OrderId,
                Offer = new OfferOrderDto
                {
                    Title = o.Offer.Title,
                    OfferId = o.Offer.OfferId
                },
                CreatedAt = o.CreatedAt,
                Status = o.Status
            }).ToListAsync();
    }

    public async Task<ActionResult> UpdateOrder(int id, OrderBody orderBody)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null) return ErrorService.BadRequest(StringRes.ErrorDuringOrderUpdate);

        order.Status = orderBody.Status;

        await _context.SaveChangesAsync();

        return SuccessService.Ok(StringRes.OrderUpdated);
    }
}