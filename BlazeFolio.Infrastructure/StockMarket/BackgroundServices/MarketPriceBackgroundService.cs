using BlazeFolio.Infrastructure.StockMarket.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazeFolio.Infrastructure.StockMarket.BackgroundServices
{
    public class MarketPriceBackgroundService : BackgroundService
    {
        private readonly ILogger<MarketPriceBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _pollingInterval = TimeSpan.FromMinutes(5); // Poll every 5 minutes

        public MarketPriceBackgroundService(
            ILogger<MarketPriceBackgroundService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Market Price Background Service is starting at {Time}", DateTime.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdatePricesAsync(stoppingToken);
                    _logger.LogInformation("Next price update scheduled at {NextUpdate}", DateTime.Now.Add(_pollingInterval));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in market price background service");
                }

                await Task.Delay(_pollingInterval, stoppingToken);
            }

            _logger.LogInformation("Market Price Background Service is stopping");
        }

        private async Task UpdatePricesAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Updating market prices");

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    // Get the symbols that we need to track
                    // This could be fetched from a user preferences repository or another service
                    var symbolsToTrack = await GetSymbolsToTrack(scope.ServiceProvider);

                    if (!symbolsToTrack.Any())
                    {
                        _logger.LogInformation("No symbols to track. Skipping update.");
                        return;
                    }

                    // Update prices in the database
                    var marketPriceService = scope.ServiceProvider.GetRequiredService<MarketPriceService>();
                    await marketPriceService.UpdatePricesAsync(symbolsToTrack);

                    // Notify connected clients through SignalR
                    await NotifyClientsOfPriceUpdates(scope.ServiceProvider, symbolsToTrack, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in price update operation");
                }
            }
        }

        private async Task<List<string>> GetSymbolsToTrack(IServiceProvider serviceProvider)
        {
            try
            {
                // Get symbols from wallet assets in the database
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BlazeFolio.Infrastructure.Persistence.LiteDbContext>();

                // Get all wallet assets from the database
                var wallets = dbContext.Wallets.FindAll().ToList();
                var symbols = new HashSet<string>();

                foreach (var wallet in wallets)
                {
                    if (wallet.Assets != null)
                    {
                        foreach (var asset in wallet.Assets)
                        {
                            if (!string.IsNullOrEmpty(asset.Symbol))
                            {
                                symbols.Add(asset.Symbol);
                            }
                        }
                    }
                }

                // If no symbols are found in wallets, use default list
                if (!symbols.Any())
                {
                    return new List<string> { "AAPL", "MSFT", "GOOGL", "AMZN", "META" };
                }

                return symbols.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting symbols to track. Using default list.");
                return new List<string> { "AAPL", "MSFT", "GOOGL", "AMZN", "META" };
            }
        }

        private async Task NotifyClientsOfPriceUpdates(IServiceProvider serviceProvider, List<string> symbols, CancellationToken stoppingToken)
        {
            try
            {
                var hubContext = serviceProvider.GetRequiredService<IHubContext<Hubs.MarketPriceHub>>();
                var marketPriceService = serviceProvider.GetRequiredService<MarketPriceService>();

                // Get the latest prices
                var latestPrices = marketPriceService.GetLatestPrices(symbols);

                // Notify all clients with updated prices
                foreach (var (symbol, price) in latestPrices)
                {
                    await hubContext.Clients.Group(symbol).SendAsync("ReceivePriceUpdate", symbol, price, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error notifying clients of price updates");
            }
        }
    }
}
