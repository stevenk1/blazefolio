using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.Models;

namespace BlazeFolio.Domain.WalletAggregate.Entities;

public class Asset : Entity<AssetId>
{
    public string Symbol { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string? LongName { get; private set; }
    public string? QuoteType { get; private set; }

    private Asset(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price)
        : base(id)
    {
        
    }

    private Asset(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price, string? longName, string? quoteType): base(id)
    {
        Symbol = symbol;
        PurchaseDate = purchaseDate;
        Quantity = quantity;
        Price = price;
        LongName = longName;
        QuoteType = quoteType;
    }


    public void UpdateMetadata(MarketMetadata metadata)
    {
        LongName = metadata.LongName;
        QuoteType = metadata.QuoteType;
    }

    public static Asset Create(string symbol, DateTime purchaseDate, int quantity, decimal price, string? longName, string? quoteType)
    {
        return new Asset(
            AssetId.CreateUnique(),
            symbol,
            purchaseDate,
            quantity,
            price,
            longName,
            quoteType);
        
    }
}
