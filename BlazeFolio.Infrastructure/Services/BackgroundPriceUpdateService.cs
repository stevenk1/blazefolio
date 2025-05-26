using System.Collections.Concurrent;
using BlazeFolio.Application.Contracts.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazeFolio.Infrastructure.Services;

public class BackgroundPriceUpdateService : BackgroundService
{
    private readonly IFusionCache _fusionCache;
    private readonly IPriceUpdateService _priceUpdateService;
    private readonly ILogger<BackgroundPriceUpdateService> _logger;
    private readonly ConcurrentDictionary<string, byte> _activeSymbols = new();
    private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(5);

    public BackgroundPriceUpdateService(
        IFusionCache fusionCache,
        IPriceUpdateService priceUpdateService,
        ILogger<BackgroundPriceUpdateService> logger)
    {
        _fusionCache = fusionCache;
        _priceUpdateService = priceUpdateService;
        _logger = logger;

        // Subscribe to the PriceUpdateService to track active symbols
        SubscribeToPriceUpdates();
    }

    private void SubscribeToPriceUpdates()
    {
        // Hijack the existing service to track which symbols are being subscribed to
        if (_priceUpdateService is PriceUpdateService updateService)
        {
            updateService.SymbolSubscribed += (sender, symbol) => 
            {
                _activeSymbols.TryAdd(symbol, 0);
                _logger.LogInformation($"Symbol {symbol} is now being tracked for automatic updates");
            };

            updateService.SymbolUnsubscribed += (sender, symbol) => 
            {
                if (!updateService.HasSubscribers(symbol))
                {
                    _activeSymbols.TryRemove(symbol, out _);
                    _logger.LogInformation($"Symbol {symbol} is no longer tracked for automatic updates");
                }
            };
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background price update service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await UpdateActivePrices(stoppingToken);
                await Task.Delay(_updateInterval, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Normal shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating prices");
                // Continue running but wait a bit before retrying
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        _logger.LogInformation("Background price update service stopped");
    }

    private async Task UpdateActivePrices(CancellationToken stoppingToken)
    {
        foreach (var symbol in _activeSymbols.Keys)
        {
            if (stoppingToken.IsCancellationRequested) break;

            try
            {
                // This will update the cache and trigger notifications
                await _fusionCache.GetCurrentPriceAsync(symbol);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update price for symbol {symbol}");
            }
        }
    }
}
