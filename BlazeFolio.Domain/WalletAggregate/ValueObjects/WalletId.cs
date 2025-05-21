using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.ValueObjects;

public sealed class WalletId : ValueObject
{
    public Guid Value { get; }

    private WalletId()
    {
        Value = Guid.Empty;
    }
    private WalletId(Guid value)
    {
        Value = value;
    }

    public static WalletId CreateUnique()
    {
        return new WalletId(Guid.NewGuid());
    }

    public static WalletId Create(Guid value)
    {
        return new WalletId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
