using System;
using BlazeFolio.Domain.WalletAggregate.Entities;
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
            
        };
    }

    // Convert to domain entity
    public Asset ToDomain()
    {
        return Asset.Create(
            Symbol,
            PurchaseDate,
            Quantity,
            Price,
            LongName,
            QuoteType
        );
    }
}
