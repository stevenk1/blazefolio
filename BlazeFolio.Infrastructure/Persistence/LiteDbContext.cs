using System;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using BlazeFolio.Infrastructure.Persistence.Models;
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
        
        _database = new LiteDatabase(connectionString, mapper);
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

    public void Dispose()
    {
        _database.Dispose();
    }
}
