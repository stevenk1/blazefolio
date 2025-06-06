using System;

namespace BlazeFolio.Infrastructure.StockMarket.Models
{
    public class MarketPriceRecord
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal? Price { get; set; }
        public DateTime Timestamp { get; set; }

        public MarketPriceRecord()
        {
        }

        public MarketPriceRecord(string symbol, decimal? price)
        {
            Symbol = symbol;
            Price = price;
            Timestamp = DateTime.UtcNow;
        }
    }
}
