using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using BlazeFolio.Application.Contracts.Infrastructure;

namespace BlazeFolio.Infrastructure.Services;

public class PriceUpdateService : IPriceUpdateService
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<Guid, Func<string, decimal, Task>>> _subscribers = new();
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    // Events to notify when symbols are subscribed to or unsubscribed from
    public event EventHandler<string>? SymbolSubscribed;
    public event EventHandler<string>? SymbolUnsubscribed;

    public bool HasSubscribers(string symbol)
    {
        return _subscribers.TryGetValue(symbol, out var symbolSubscribers) && !symbolSubscribers.IsEmpty;
    }

    public async Task<Guid> SubscribeAsync(string symbol, Func<string, decimal, Task> callback)
    {
        await _semaphore.WaitAsync();
        try
        {
            var subscriptionId = Guid.NewGuid();
            var isNewSymbol = false;

            if (!_subscribers.TryGetValue(symbol, out var symbolSubscribers))
            {
                symbolSubscribers = new ConcurrentDictionary<Guid, Func<string, decimal, Task>>();
                _subscribers[symbol] = symbolSubscribers;
                isNewSymbol = true;
            }

            symbolSubscribers[subscriptionId] = callback;

            // If this is a new symbol, trigger the event
            if (isNewSymbol)
            {
                SymbolSubscribed?.Invoke(this, symbol);
            }

            return subscriptionId;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task UnsubscribeAsync(string symbol, Guid subscriptionId)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_subscribers.TryGetValue(symbol, out var symbolSubscribers))
            {
                symbolSubscribers.TryRemove(subscriptionId, out _);

                // If no subscribers left for this symbol, remove the symbol entry
                if (symbolSubscribers.IsEmpty)
                {
                    _subscribers.TryRemove(symbol, out _);

                    // Trigger the event since we no longer have subscribers for this symbol
                    SymbolUnsubscribed?.Invoke(this, symbol);
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task NotifyPriceUpdateAsync(string symbol, decimal newPrice)
    {
        if (_subscribers.TryGetValue(symbol, out var symbolSubscribers))
        {
            var notificationTasks = new List<Task>();

            foreach (var callback in symbolSubscribers.Values)
            {
                try 
                {
                    notificationTasks.Add(callback(symbol, newPrice));
                }
                catch (Exception ex)
                {
                    // Log the exception but continue with other notifications
                    Console.WriteLine($"Error notifying subscriber for {symbol}: {ex.Message}");
                }
            }

            if (notificationTasks.Any())
            {
                await Task.WhenAll(notificationTasks);
            }
        }
    }
}
