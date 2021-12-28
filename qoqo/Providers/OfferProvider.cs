using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Model;

namespace qoqo.Providers;

public class OfferProvider
{
    private readonly QoqoContext _context;

    public OfferProvider(QoqoContext context)
    {
        _context = context;
    }

    public async Task<Offer?> GetCurrentOffer()
    {
        var today = DateTime.Today;
        var offer = await _context.Offers.FirstOrDefaultAsync(o => o.StartAt <= today && o.EndAt >= today && !o.IsDraft);

        return offer;
    }

    public async Task<DashboardDto?> GetDashboard()
    {
        var offer = await GetCurrentOffer();
        if (offer == null)
        {
            return null;
        }
        

        var clickCount = await _context.Clicks.Where(o => o.OfferId == offer.Id).CountAsync();
        var activeUserCount = await _context.Clicks.Where(o => o.OfferId == offer.Id).GroupBy(c => c.UserId).CountAsync();
        var order = await _context.Orders
            .Where(o => o.OfferId == offer.Id)
            .Include(o => o.User)
            .Select(o => OrderDashboardDto.FromOrder(o))
            .FirstOrDefaultAsync();
        
        var dashboard = new DashboardDto
        {
            OfferId = offer.Id,
            OfferTitle = offer.Title,
            IsOver = offer.IsOver,
            ClickObjective = offer.ClickObjective,
            ClickCount = clickCount,
            CountOfActiveUser = activeUserCount,
            EndAt = offer.EndAt,
            Order = order
        };

        return dashboard;
    }
}