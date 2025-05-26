namespace BlazeFolio.Application.Contracts.Infrastructure;

public interface IPriceUpdateService
{
    Task<Guid> SubscribeAsync(string symbol, Func<string, decimal, Task> callback);
    Task UnsubscribeAsync(string symbol, Guid subscriptionId);
    Task NotifyPriceUpdateAsync(string symbol, decimal newPrice);
}
