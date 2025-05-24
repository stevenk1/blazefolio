using BlazeFolio.Application.Tests.Testing;
using BlazeFolio.Application.Wallets.Features.Queries;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.Entities;
using CSharpFunctionalExtensions;

namespace BlazeFolio.Application.Tests.Wallets.Features.Queries;

public class GetAllWalletsHandlerTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    private readonly GetAllWalletsHandler _handler;

    public GetAllWalletsHandlerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _handler = new GetAllWalletsHandler(_fixture.WalletRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnAllWallets()
    {
        // Arrange
        var wallet1 = Wallet.CreateNew("Test Wallet 1", [1, 2, 3]);
        var wallet2 = Wallet.CreateNew("Test Wallet 2", [4, 5, 6]);
        
        // Add assets to wallets
        var asset1 = Asset.Create("BTC", DateTime.UtcNow.AddDays(-10), 2, 50000m);
        var asset2 = Asset.Create("ETH", DateTime.UtcNow.AddDays(-5), 5, 3000m);

        wallet1.AddAsset(asset1);
        wallet2.AddAsset(asset2);

        await _fixture.WalletRepository.AddAsync(wallet1);
        await _fixture.WalletRepository.AddAsync(wallet2);

        var query = new GetAllWallets();

        // Act
        Result<List<Wallet>> result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Count.Should().BeGreaterThanOrEqualTo(2);
        result.Value.Should().Contain(w => w.Id == wallet1.Id);
        result.Value.Should().Contain(w => w.Id == wallet2.Id);

        // Check if assets are returned with wallets
        var retrievedWallet1 = result.Value.First(w => w.Id == wallet1.Id);
        retrievedWallet1.Assets.Should().HaveCount(1);
        retrievedWallet1.Assets.Should().Contain(a => a.Symbol == "BTC" && a.Quantity == 2 && a.Price == 50000m);

        var retrievedWallet2 = result.Value.First(w => w.Id == wallet2.Id);
        retrievedWallet2.Assets.Should().HaveCount(1);
        retrievedWallet2.Assets.Should().Contain(a => a.Symbol == "ETH" && a.Quantity == 5 && a.Price == 3000m);
    }
}
