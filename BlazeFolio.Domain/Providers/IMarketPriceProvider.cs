namespace BlazeFolio.Domain.Providers
{
    public interface IMarketPriceProvider
    {
        /// <summary>
        /// Gets the current price for a given stock symbol
        /// </summary>
        /// <param name="symbol">The stock symbol to look up</param>
        /// <returns>The current price or null if not found</returns>
        Task<decimal?> GetCurrentPriceAsync(string symbol);

        /// <summary>
        /// Gets the current prices for multiple stock symbols in a single batch request
        /// </summary>
        /// <param name="symbols">The stock symbols to look up</param>
        /// <returns>Dictionary of symbols and their current prices</returns>
        Task<Dictionary<string, decimal?>> GetCurrentPricesAsync(IEnumerable<string> symbols);
    }
}
