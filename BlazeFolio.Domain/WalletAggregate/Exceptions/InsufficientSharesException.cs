using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate.Exceptions;

public class InsufficientSharesException : DomainException
{
    public InsufficientSharesException(AssetId assetId, int requested, int available)
        : base($"Insufficient shares available. Requested: {requested}, Available: {available}")
    {
        AssetId = assetId;
        Requested = requested;
        Available = available;
    }

    public AssetId AssetId { get; }
    public int Requested { get; }
    public int Available { get; }
}
