using System;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using LiteDB;

namespace BlazeFolio.Infrastructure.Persistence.Models;
using Domain.WalletAggregate.ValueObjects;

public class WalletModel
{
    [BsonId]

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public byte[] Picture{ get; set; } = [];

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
            Picture = wallet.Picture.Value
        };
    }

    // Convert to domain entity
    public Wallet ToDomain()
    {
        return new Wallet(
            WalletId.Create(Id),
            Name,
            Domain.WalletAggregate.ValueObjects.Picture.Create(Picture)
        );
    }
}
