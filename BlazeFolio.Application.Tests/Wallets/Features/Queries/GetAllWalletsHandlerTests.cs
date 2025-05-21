using BlazeFolio.Application.Tests.Testing;
using BlazeFolio.Application.Wallets.Features.Queries;
using BlazeFolio.Domain.WalletAggregate;
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
    }
}
