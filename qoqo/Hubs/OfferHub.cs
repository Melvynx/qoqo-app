using Microsoft.AspNetCore.SignalR;

namespace qoqo.Hubs;

public class OfferHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task OnConnected(string user)
    {
        await Clients.All.SendAsync("UserConnected", user);
    }
}