using Microsoft.EntityFrameworkCore;
using qoqo.Model;

namespace qoqo.Providers;

public class ClickProvider
{
    private readonly QoqoContext _context;

    public ClickProvider(QoqoContext context)
    {
        _context = context;
    }

    public async Task<Click> Add(int userId, int offerId)
    {
        var newClick = new Click
        {
            UserId = userId,
            OfferId = offerId
        };
        
        var click = await _context.Clicks.AddAsync(newClick);
        await _context.SaveChangesAsync();
        return click.Entity;
    }
    
    public async Task<int> GetCountForOffer(int offerId)
    {
        var click = await _context.Clicks.CountAsync(c => c.OfferId == offerId);
        return click;
    }
}