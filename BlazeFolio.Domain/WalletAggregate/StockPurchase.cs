using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate;

public class StockPurchase : Entity<StockPurchaseId>
{
    public string Symbol { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private StockPurchase(StockPurchaseId id, string symbol, DateTime purchaseDate, int quantity, decimal price)
        : base(id)
    {
        Symbol = symbol;
        PurchaseDate = purchaseDate;
        Quantity = quantity;
        Price = price;
    }

    public static StockPurchase Create(string symbol, DateTime purchaseDate, int quantity, decimal price)
    {
        return new StockPurchase(
            StockPurchaseId.CreateUnique(),
            symbol,
            purchaseDate,
            quantity,
            price);
    }
}
