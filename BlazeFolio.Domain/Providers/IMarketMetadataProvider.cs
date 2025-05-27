using BlazeFolio.Domain.Models;

namespace BlazeFolio.Domain.Providers
{
    public interface IMarketMetadataProvider
    {
        /// <summary>
        /// Gets metadata for a given stock symbol
        /// </summary>
        /// <param name="symbol">The stock symbol to look up</param>
        /// <returns>The market metadata or null if not found</returns>
        Task<MarketMetadata?> GetMetadataAsync(string symbol);

        /// <summary>
        /// Gets metadata for multiple stock symbols in a single batch request
        /// </summary>
        /// <param name="symbols">The stock symbols to look up</param>
        /// <returns>Dictionary of symbols and their metadata</returns>
        Task<Dictionary<string, MarketMetadata?>> GetMetadataAsync(IEnumerable<string> symbols);
    }
}
