using BlazeFolio.Domain.Providers;
using YahooFinanceApi;

namespace BlazeFolio.Infrastructure.StockMarket.Services
{
    public class YahooFinanceMarketPriceProvider : IMarketPriceProvider
    {
        public async Task<decimal?> GetCurrentPriceAsync(string symbol)
        {
            try
            {
                // Get current security price from Yahoo Finance API
                var securities = await Yahoo.Symbols(symbol).Fields(Field.RegularMarketPrice).QueryAsync();

                if (securities.TryGetValue(symbol, out var security))
                {
                    var price = security[Field.RegularMarketPrice];
                    if (price != null)
                    {
                        return Convert.ToDecimal(price);
                    }
                }

                return null;
            }
            catch (Exception)
            {
                // Log exception if needed
                return null;
            }
        }

        public async Task<Dictionary<string, decimal?>> GetCurrentPricesAsync(IEnumerable<string> symbols)
        {
            var result = new Dictionary<string, decimal?>();
            var symbolsList = symbols.ToList();

            if (!symbolsList.Any())
            {
                return result;
            }

            try
            {
                // Get current security prices for multiple symbols in a single batch request
                var securities = await Yahoo.Symbols(symbolsList.ToArray())
                    .Fields(Field.RegularMarketPrice)
                    .QueryAsync();

                foreach (var symbol in symbolsList)
                {
                    if (securities.TryGetValue(symbol, out var security))
                    {
                        var price = security[Field.RegularMarketPrice];
                        if (price != null)
                        {
                            result[symbol] = Convert.ToDecimal(price);
                        }
                        else
                        {
                            result[symbol] = null;
                        }
                    }
                    else
                    {
                        result[symbol] = null;
                    }
                }
            }
            catch (Exception ex)
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
