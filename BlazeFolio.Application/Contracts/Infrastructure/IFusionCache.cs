namespace BlazeFolio.Application.Contracts.Infrastructure;

public interface IFusionCache
{
    Task<decimal> GetCurrentPriceAsync(string symbol);
}
