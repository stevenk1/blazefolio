using Microsoft.AspNetCore.SignalR;

namespace BlazeFolio.Hubs
{
    public class MarketPriceHub : Hub
    {
        public async Task JoinSymbolGroup(string symbol)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, symbol);
        }

        public async Task LeaveSymbolGroup(string symbol)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, symbol);
        }
    }
}
