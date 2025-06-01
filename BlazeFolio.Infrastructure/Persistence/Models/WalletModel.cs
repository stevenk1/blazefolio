using BlazeFolio.Domain.WalletAggregate;
using LiteDB;

namespace BlazeFolio.Infrastructure.Persistence.Models;

using Domain.WalletAggregate.ValueObjects;

public class WalletModel
{
    [BsonId] public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte[] Picture { get; set; } = [];
    public WalletType Type { get; set; } = WalletType.StockExchange;
    public List<AssetModel> Assets { get; set; } = new();

    // Parameterless constructor required by LiteDB
    public WalletModel()
    {
    }

    // Create from domain entity
    public static WalletModel FromDomain(Wallet wallet)
    {
        return new WalletModel
        {
            Id = wallet.Id.Value,
            Name = wallet.Name,
            Picture = wallet.Picture.Value,
            Type = wallet.Type,
            Assets = wallet.Assets.Select(AssetModel.FromDomain).ToList()
        };
    }

    // Convert to domain entity
    public Wallet ToDomain()
    {
        var wallet = Wallet.FromPersistence(WalletId.Create(Id), Name,
            Domain.WalletAggregate.ValueObjects.Picture.Create(Picture), Type);
        
        foreach (var asset in Assets)
        {
            wallet.AddAsset(asset.ToDomain());
        }

        return wallet;
    }
}