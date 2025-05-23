using BlazeFolio.Domain.Common;

namespace BlazeFolio.Domain.WalletAggregate.ValueObjects;

public sealed class Picture : ValueObject
{
    public byte[] Value { get; }
    
    private Picture()
    {
        Value = [];
    }

    private Picture(byte[] value)
    {
        Value = value;
    }

    public static Picture Create(byte[] value)
    {
        // Add validation logic if necessary
        return new Picture(value);
    }

    public static Picture Empty()
    {
        return new Picture([]);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        // For byte array equality, we need to compare each byte
        foreach (var b in Value)
        {
            yield return b;
        }
    }
}
