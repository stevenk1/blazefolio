using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.ValueObjects;

public sealed class AssetId : ValueObject
{
    public Guid Value { get; }

    private AssetId(Guid value)
    {
        Value = value;
    }

    public static AssetId CreateUnique()
    {
        return new AssetId(Guid.NewGuid());
    }

    public static AssetId Create(Guid value)
    {
        return new AssetId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
