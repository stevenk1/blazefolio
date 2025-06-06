using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazeFolio.Domain.Providers;
using BlazeFolio.Infrastructure.Persistence;
using BlazeFolio.Infrastructure.StockMarket.Models;
using Microsoft.Extensions.Logging;

namespace BlazeFolio.Infrastructure.StockMarket.Services
{
    public class MarketPriceService
    {
        private readonly LiteDbContext _dbContext;
        private readonly IMarketPriceProvider _marketPriceProvider;
        private readonly ILogger<MarketPriceService> _logger;

        public MarketPriceService(
            LiteDbContext dbContext,
            IMarketPriceProvider marketPriceProvider,
            ILogger<MarketPriceService> logger)
        {
            _dbContext = dbContext;
            _marketPriceProvider = marketPriceProvider;
            _logger = logger;
        }

        public async Task UpdatePricesAsync(IEnumerable<string> symbols)
        {
            try
            {
                var symbolsList = symbols.ToList();
                if (!symbolsList.Any())
                {
                    return;
                }

                _logger.LogInformation($"Updating prices for {symbolsList.Count} symbols");

                // Get current prices from the provider
                var prices = await _marketPriceProvider.GetCurrentPricesAsync(symbolsList);

                // Store prices in the database
                foreach (var (symbol, price) in prices)
                {
                    var record = new MarketPriceRecord(symbol, price);
                    _dbContext.MarketPrices.Insert(record);
                }

                _logger.LogInformation("Price update completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating market prices");
            }
        }

        public Dictionary<string, decimal?> GetLatestPrices(IEnumerable<string> symbols)
        {
            var result = new Dictionary<string, decimal?>();
            var symbolsList = symbols.ToList();

            foreach (var symbol in symbolsList)
            {
                // Get the most recent price record for each symbol
                var latestPrice = _dbContext.MarketPrices
                    .Query()
                    .Where(p => p.Symbol == symbol)
                    .OrderByDescending(p => p.Timestamp)
                    .FirstOrDefault();

                result[symbol] = latestPrice?.Price;
            }

            return result;
        }
    }
}
