using BlazeFolio.Domain.Models;
using BlazeFolio.Domain.Providers;
using YahooFinanceApi;

namespace BlazeFolio.Infrastructure.StockMarket.Services
{
    public class YahooFinanceMarketMetadataProvider : IMarketMetadataProvider
    {
        public async Task<MarketMetadata?> GetMetadataAsync(string symbol)
        {
            try
            {
                // Get security metadata from Yahoo Finance API
                var securities = await Yahoo.Symbols(symbol)
                    .Fields(Field.LongName, Field.QuoteType, Field.Currency)
                    .QueryAsync();

                if (securities.TryGetValue(symbol, out var security))
                {
                    var metadata = new MarketMetadata();

                    var longName = security[Field.LongName];
                    if (longName != null)
                    {
                        metadata.LongName = longName.ToString();
                    }

                    var quoteType = security[Field.QuoteType];
                    if (quoteType != null)
                    {
                        metadata.QuoteType = quoteType.ToString();
                    }

                    var currency = security[Field.Currency];
                    if (currency != null)
                    {
                        metadata.Currency = currency.ToString();
                    }

                    return metadata;
                }

                return null;
            }
            catch (Exception)
            {
                // Log exception if needed
                return null;
            }
        }

        public async Task<Dictionary<string, MarketMetadata?>> GetMetadataAsync(IEnumerable<string> symbols)
        {
            var result = new Dictionary<string, MarketMetadata?>();
            var symbolsList = symbols.ToList();

            if (!symbolsList.Any())
            {
                return result;
            }

            try
            {
                // Get metadata for multiple symbols in a single batch request
                var securities = await Yahoo.Symbols(symbolsList.ToArray())
                    .Fields(Field.LongName, Field.QuoteType, Field.Currency)
                    .QueryAsync();

                foreach (var symbol in symbolsList)
                {
                    if (securities.TryGetValue(symbol, out var security))
                    {
                        var metadata = new MarketMetadata();

                        var longName = security[Field.LongName];
                        if (longName != null)
                        {
                            metadata.LongName = longName.ToString();
                        }

                        var quoteType = security[Field.QuoteType];
                        if (quoteType != null)
                        {
                            metadata.QuoteType = quoteType.ToString();
                        }

                        var currency = security[Field.Currency];
                        if (currency != null)
                        {
                            metadata.Currency = currency.ToString();
                        }

                        result[symbol] = metadata;
                    }
                    else
                    {
                        result[symbol] = null;
                    }
                }
            }
            catch (Exception)
            {
                // Log exception if needed
                // Return any missing symbols as null
                foreach (var symbol in symbolsList)
                {
                    result.TryAdd(symbol, null);
                }
            }

            return result;
        }
    }
}
