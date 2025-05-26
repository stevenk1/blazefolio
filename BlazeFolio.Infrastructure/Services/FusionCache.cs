using BlazeFolio.Application.Contracts.Infrastructure;
using System.Collections.Concurrent;

namespace BlazeFolio.Infrastructure.Services;

public class FusionCache : IFusionCache
{
    private readonly ConcurrentDictionary<string, decimal> _cache = new();
    private readonly Random _random = new();
    private readonly IPriceUpdateService _priceUpdateService;

    public FusionCache(IPriceUpdateService priceUpdateService)
    {
        _priceUpdateService = priceUpdateService;
    }

    // In a real implementation, this would connect to a real price source
    // For now, we're simulating prices with random values
    public async Task<decimal> GetCurrentPriceAsync(string symbol)
    {
        // If we already have a price in cache, return it with a small random change
        if (_cache.TryGetValue(symbol, out decimal cachedPrice))
        {
            // Apply a small random change (-5% to +5%)
            var changePercent = (_random.NextDouble() * 0.1) - (double)0.05m;
            var newPrice = Math.Max(0.01m, cachedPrice * (1 + (decimal)changePercent));
            _cache[symbol] = newPrice;

            // Notify subscribers about the price update
            await _priceUpdateService.NotifyPriceUpdateAsync(symbol, newPrice);

            return newPrice;
        }

        // For new symbols, generate a random price between $10 and $1000
        var initialPrice = (decimal)(_random.NextDouble() * 990 + 10);
        _cache[symbol] = initialPrice;

        // Simulate network delay
        await Task.Delay(100);

        // Notify subscribers about the initial price
        await _priceUpdateService.NotifyPriceUpdateAsync(symbol, initialPrice);

        return initialPrice;
    }
}
