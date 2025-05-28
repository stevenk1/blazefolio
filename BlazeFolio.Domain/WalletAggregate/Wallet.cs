using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Domain.WalletAggregate;

public class Wallet(WalletId id, string name, Picture picture, WalletType type = WalletType.StockExchange) : AggregateRoot<WalletId>(id)
{
    public string Name { get; private set; } = name;
    public Picture Picture { get; private set; } = picture;
    public WalletType Type { get; private set; } = type;
    private readonly List<Asset> _assets = new();
    public IReadOnlyList<Asset> Assets => _assets.AsReadOnly();

    public static Wallet CreateNew(string name, byte[] pictureData, WalletType type = WalletType.StockExchange)
    {
        var wallet= new Wallet(
            WalletId.CreateUnique(),
            name,
            Picture.Create(pictureData),
            type);
        return wallet;
    }
    
    public void AddAsset(Asset asset)
    {
        _assets.Add(asset);
    }

    public void RemoveAsset(Asset asset)
    {
        _assets.Remove(asset);
    }
}