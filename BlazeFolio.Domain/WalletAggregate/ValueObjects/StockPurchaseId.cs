using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.ValueObjects;

public sealed class StockPurchaseId : ValueObject
{
    public Guid Value { get; }

    private StockPurchaseId(Guid value)
    {
        Value = value;
    }

    public static StockPurchaseId CreateUnique()
    {
        return new StockPurchaseId(Guid.NewGuid());
    }

    public static StockPurchaseId Create(Guid value)
    {
        return new StockPurchaseId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
