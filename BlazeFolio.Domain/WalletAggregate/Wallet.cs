using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.Exceptions;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate;

public class Wallet : AggregateRoot<WalletId>
{
    private Wallet(WalletId id, string name, Picture picture, WalletType type) : base(id)
    {
        Name = name;
        Picture = picture;
        Type = type;
    }

    public string Name { get; private set; }
    public Picture Picture { get; private set; }
    public WalletType Type { get; private set; }
    private readonly List<Asset> _assets = new();
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();

    public static Wallet CreateNew(string name, byte[] pictureData, WalletType type = WalletType.StockExchange)
    {
        var wallet = new Wallet(
            WalletId.CreateUnique(),
            name,
            Picture.Create(pictureData),
            type);
        return wallet;
    }

    public static Wallet FromPersistence(WalletId id, string name, Picture picture, WalletType type = WalletType.StockExchange)
    {
        return new Wallet(id, name, picture, type);
    }
    
    public void AddAsset(Asset asset)
    {
        _assets.Add(asset);
    }

    public void RemoveAsset(Asset asset)
    {
        _assets.Remove(asset);
    }

    public void SellAsset(AssetId assetId, int quantity, DateTime saleDate, decimal price)
    {
        // Find the asset to sell
        var asset = _assets.FirstOrDefault(a => a.Id == assetId);
        if (asset == null)
        {
            throw new AssetNotFoundException(assetId);
        }

        // Validate quantity
        if (asset.Quantity < quantity)
        {
            throw new InsufficientSharesException(assetId, quantity, asset.Quantity);
        }

        // Remove asset from wallet
        _assets.Remove(asset);

        // If not selling all shares, create a new asset with remaining shares
        if (asset.Quantity > quantity)
        {
            var remainingAsset = Asset.Create(
                asset.Symbol,
                asset.PurchaseDate,
                asset.Quantity - quantity,
                asset.Price,
                asset.LongName,
                asset.QuoteType,
                asset.Currency);

            _assets.Add(remainingAsset);
        }

    }
}