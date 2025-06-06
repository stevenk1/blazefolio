using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.Models;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate.Entities;

public class Asset : Entity<AssetId>
{
    public string Symbol { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public string? LongName { get; private set; }
    public string? QuoteType { get; private set; }
    public string? Currency { get; private set; }

    private Asset(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price)
        : base(id)
    {
        
    }

    private Asset(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price, string? longName, string? quoteType, string? currency): base(id)
    {
        Symbol = symbol;
        PurchaseDate = purchaseDate;
        Quantity = quantity;
        Price = price;
        LongName = longName;
        QuoteType = quoteType;
        Currency = currency;
    }


    public void UpdateMetadata(MarketMetadata metadata)
    {
        LongName = metadata.LongName;
        QuoteType = metadata.QuoteType;
        Currency = metadata.Currency;
    }

    public static Asset Create(string symbol, DateTime purchaseDate, int quantity, decimal price)
    {
        ValidatePurchaseDate(purchaseDate);

        return new Asset(
            AssetId.CreateUnique(),
            symbol,
            purchaseDate,
            quantity,
            price,
            null, 
            null,
            null);
        
    }
    
    public static Asset Create(string symbol, DateTime purchaseDate, int quantity, decimal price,string? longname,string? quoteType,string? currency)
    {
        ValidatePurchaseDate(purchaseDate);

        return new Asset(
            AssetId.CreateUnique(),
            symbol,
            purchaseDate,
            quantity,
            price,
            longname, 
            quoteType,
            currency);
        
    }

    private static void ValidatePurchaseDate(DateTime purchaseDate)
    {
        // Check if purchase date is in the future
        if (purchaseDate > DateTime.Today)
        {
            throw new Exceptions.FuturePurchaseDateException(purchaseDate, DateTime.Today);
        }
    }

    public static Asset FromPersistence(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price, string? longName = null, string? quoteType = null, string? currency = null)
    {
        return new Asset(
            id,
            symbol,
            purchaseDate,
            quantity,
            price,
            longName,
            quoteType,
            currency);
    }
}
