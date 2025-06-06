using System;
using System.IO;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using BlazeFolio.Infrastructure.Persistence.Models;
using BlazeFolio.Infrastructure.StockMarket.Models;
using LiteDB;

namespace BlazeFolio.Infrastructure.Persistence;

public class LiteDbContext : IDisposable
{
    private readonly LiteDatabase _database;
    
    public LiteDbContext(string connectionString)
    {
        var mapper = new BsonMapper();
        
        // Register custom mappings for value objects
        RegisterCustomMappings(mapper);
        
        // Check if the database file exists and create directory if needed
        EnsureDatabaseDirectoryExists(connectionString);
        
        _database = new LiteDatabase(connectionString, mapper);
        
        // Initialize database collections if needed
        InitializeCollections();
    }

    private static void EnsureDatabaseDirectoryExists(string connectionString)
    {
        try
        {
            // Extract file path from connection string
            // Connection string format is typically: "filename=path/to/database.db;..."
            var parts = connectionString.Split(';');
            string filePath = null;
            
            foreach (var part in parts)
            {
                if (part.Trim().StartsWith("filename=", StringComparison.OrdinalIgnoreCase))
                {
                    filePath = part.Substring("filename=".Length).Trim();
                    break;
                }
            }
            
            if (!string.IsNullOrEmpty(filePath))
            {
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"Error ensuring database directory exists: {ex.Message}");
        }
    }

    private void InitializeCollections()
    {
        // Ensure collections are created
        var wallets = _database.GetCollection<WalletModel>("wallets");
        var marketPrices = _database.GetCollection<MarketPriceRecord>("marketPrices");

        // Create any necessary indexes
        wallets.EnsureIndex(x => x.Id);
        marketPrices.EnsureIndex(x => x.Symbol);
        marketPrices.EnsureIndex(x => x.Timestamp);
    }

    private static void RegisterCustomMappings(BsonMapper mapper)
    {
        // WalletId mapping
        mapper.RegisterType<WalletId>(
            serialize: walletId => walletId.Value, 
            deserialize: bson => WalletId.Create((Guid)bson)
        );
        
        // Picture mapping (optional, since we're storing as raw bytes in the model)
        mapper.RegisterType<Picture>(
            serialize: picture => picture.Value,
            deserialize: bson => Picture.Create((byte[])bson)
        );
    }

    // Collection accessors
    public ILiteCollection<WalletModel> Wallets => _database.GetCollection<WalletModel>("wallets");
    public ILiteCollection<MarketPriceRecord> MarketPrices => _database.GetCollection<MarketPriceRecord>("marketPrices");

    public void Dispose()
    {
        _database.Dispose();
    }
}
