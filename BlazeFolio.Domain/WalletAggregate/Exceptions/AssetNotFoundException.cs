using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate.Exceptions;

public class AssetNotFoundException : DomainException
{
    public AssetNotFoundException(AssetId assetId)
        : base($"Asset with id: {assetId.Value} not found in wallet.")
    {
        AssetId = assetId;
    }

    public AssetId AssetId { get; }
}
