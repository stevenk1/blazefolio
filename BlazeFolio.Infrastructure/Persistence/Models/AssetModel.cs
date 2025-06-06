using System;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using LiteDB;

namespace BlazeFolio.Infrastructure.Persistence.Models;

public class AssetModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    public string? QuoteType { get; set; }

    public string? LongName { get; set; }
    
    public string? Currency { get; set; }
    
    // Parameterless constructor required by LiteDB
    public AssetModel()
    {
    }
    
    // Create from domain entity
    public static AssetModel FromDomain(Asset asset)
    {
        return new AssetModel
        {
            Id = asset.Id.Value,
            Symbol = asset.Symbol,
            Quantity = asset.Quantity,
            Price = asset.Price,
            PurchaseDate = asset.PurchaseDate,
            LongName = asset.LongName,
            QuoteType= asset.QuoteType,
            Currency = asset.Currency,
        };
    }

    // Convert to domain entity
    public Asset ToDomain()
    {
        return Asset.FromPersistence(
              AssetId.Create(Id),
            Symbol,
            PurchaseDate,
            Quantity,
            Price,
            LongName,
            QuoteType,
            Currency
        );
    }
}
