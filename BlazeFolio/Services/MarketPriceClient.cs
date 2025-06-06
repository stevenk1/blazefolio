using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace BlazeFolio.Services
{
    public class MarketPriceClient : IAsyncDisposable
    {
        private readonly ILogger<MarketPriceClient> _logger;
        private HubConnection _hubConnection;
        private readonly NavigationManager _navigationManager;

        public event Action<string, decimal?> OnPriceUpdate;

        public MarketPriceClient(ILogger<MarketPriceClient> logger, NavigationManager navigationManager)
        {
            _logger = logger;
            _navigationManager = navigationManager;
        }

        public async Task InitializeAsync()
        {
            if (_hubConnection != null)
            {
                return;
            }

            // Create the connection
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_navigationManager.ToAbsoluteUri("/marketPriceHub"))
                .WithAutomaticReconnect()
                .Build();

            // Set up handlers
            _hubConnection.On<string, decimal?>("ReceivePriceUpdate", (symbol, price) =>
            {
                _logger.LogInformation($"Received price update for {symbol}: {price}");
                OnPriceUpdate?.Invoke(symbol, price);
            });

            // Start the connection
            await _hubConnection.StartAsync();
            _logger.LogInformation("Market price hub connection started");
        }

        public async Task SubscribeToSymbolAsync(string symbol)
        {
            if (_hubConnection?.State != HubConnectionState.Connected)
            {
                await InitializeAsync();
            }

            await _hubConnection.InvokeAsync("JoinSymbolGroup", symbol);
            _logger.LogInformation($"Subscribed to symbol: {symbol}");
        }

        public async Task UnsubscribeFromSymbolAsync(string symbol)
        {
            if (_hubConnection?.State == HubConnectionState.Connected)
            {
                await _hubConnection.InvokeAsync("LeaveSymbolGroup", symbol);
                _logger.LogInformation($"Unsubscribed from symbol: {symbol}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
