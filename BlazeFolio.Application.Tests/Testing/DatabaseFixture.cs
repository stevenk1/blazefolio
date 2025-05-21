using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Infrastructure.Persistence;
using BlazeFolio.Infrastructure.Repositories;
using System;

namespace BlazeFolio.Application.Tests.Testing;

public class DatabaseFixture : IDisposable
{
    public LiteDbContext DbContext { get; }
    public IWalletRepository WalletRepository { get; }

    public DatabaseFixture()
    {
        // Use an in-memory database for testing
        string connectionString = "filename=:memory:;";
        DbContext = new LiteDbContext(connectionString);
        WalletRepository = new WalletRepository(DbContext);
    }

    public void Dispose()
    {
        DbContext.Dispose();
    }
}
