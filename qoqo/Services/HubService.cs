using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using qoqo.DataTransferObjects;
using qoqo.Hubs;
using qoqo.Model;

namespace qoqo.Services;

public class HubService
{
    private readonly QoqoContext _context;
    private readonly IHubContext<OfferHub> _hubContext;

    public HubService(QoqoContext qoqoContext, IHubContext<OfferHub> hubContext)
    {
        _context = qoqoContext;
        _hubContext = hubContext;
    }

    public async Task<ClickEventResult> Finish(ClickDto clickDto, Click click)
    {
        var order = new Order
        {
            OfferId = click.OfferId,
            UserId = click.UserId,
            Status = OrderStatus.PENDING
        };
        await _context.Orders.AddAsync(order);

        var offer = await _context.Offers.FindAsync(click.OfferId);
        if (offer == null) return new ClickEventResult(false);

        var user = await _context.Users
            .Select(u => new {u.UserId, u.UserName})
            .FirstAsync(u => u.UserId == click.UserId);

        offer.IsOver = true;
        offer.WinnerText = await GetWinnerSentence(user.UserId, offer.OfferId, offer.ClickObjective, user.UserName);

        await _context.SaveChangesAsync();

        var result = new ClickEventFinishResult(user.UserId, user.UserName, offer.WinnerText, clickDto.ClickCount);
        await SendAsync("FINISH", result);
        return new ClickEventResult(true);
    }

    public async Task<ClickEventResult> Click(ClickDto clickDto)
    {
        await SendAsync("CLICK", clickDto);
        return new ClickEventResult(false);
    }

    private async Task<string> GetWinnerSentence(int userId, int offerId, int clickObjective, string userName)
    {
        var winnerClickCount = await _context.Clicks
            .Where(c => c.UserId == userId && c.OfferId == offerId)
            .CountAsync();

        var winnerPercentage = winnerClickCount * 100 / clickObjective;

        return
            $"<b>@{userName}</b> has wine the challenge! He is the <b>{clickObjective}</b> to click on the button. The click <b>{winnerClickCount} time</b>, so he reach <b>{winnerPercentage}%</b> of the total click.";
    }

    private async Task SendAsync(string key, object obj)
    {
        await _hubContext.Clients.All.SendAsync(key, JsonService.Serialize(obj));
    }
}