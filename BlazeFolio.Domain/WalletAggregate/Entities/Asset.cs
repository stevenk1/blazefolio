using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.Entities;

public class Asset : Entity<AssetId>
{
    public string Symbol { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private Asset(AssetId id, string symbol, DateTime purchaseDate, int quantity, decimal price)
        : base(id)
    {
        Symbol = symbol;
        PurchaseDate = purchaseDate;
        Quantity = quantity;
        Price = price;
    }

    public static Asset Create(string symbol, DateTime purchaseDate, int quantity, decimal price)
    {
        return new Asset(
            AssetId.CreateUnique(),
            symbol,
            purchaseDate,
            quantity,
            price);
    }
}
