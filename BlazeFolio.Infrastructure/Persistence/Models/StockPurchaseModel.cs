using System;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using LiteDB;

namespace BlazeFolio.Infrastructure.Persistence.Models;

public class StockPurchaseModel
{
    [BsonId]
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    
    // Parameterless constructor required by LiteDB
    public StockPurchaseModel()
    {
    }
    
    // Create from domain entity
    public static StockPurchaseModel FromDomain(StockPurchase stockPurchase)
    {
        return new StockPurchaseModel
        {
            Id = stockPurchase.Id.Value,
            Symbol = stockPurchase.Symbol,
            Quantity = stockPurchase.Quantity,
            Price = stockPurchase.Price,
            PurchaseDate = stockPurchase.PurchaseDate
        };
    }
    
    // Convert to domain entity
    public StockPurchase ToDomain()
    {
        return  StockPurchase.Create(
            Symbol,
            PurchaseDate,
            Quantity,
            Price
        );
    }
}
