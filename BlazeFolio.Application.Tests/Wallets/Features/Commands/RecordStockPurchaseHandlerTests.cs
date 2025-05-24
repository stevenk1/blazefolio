using BlazeFolio.Application.Tests.Testing;
using BlazeFolio.Application.Wallets.Features.Commands;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Application.Tests.Wallets.Features.Commands;

public class RecordStockPurchaseHandlerTests : IClassFixture<DatabaseFixture>
{
    private readonly RecordAssetTransactionHandler _handler;
    private readonly DatabaseFixture _fixture;

    public RecordStockPurchaseHandlerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
        _handler = new RecordAssetTransactionHandler(_fixture.WalletRepository);
    }

    [Fact]
    public async Task Handle_ShouldRecordStockPurchase_WhenWalletExists()
    {
        // Arrange
        var wallet = Wallet.CreateNew("Test Wallet", [1, 2, 3]);
        var addedWallet = await _fixture.WalletRepository.AddAsync(wallet);

        var command = new RecordAssetTransaction(
            addedWallet.Id,
            "AAPL",
            DateTime.UtcNow,
            150.50m,
            3
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        // Verify stock was added
        var updatedWallet = await _fixture.WalletRepository.GetByIdAsync(addedWallet.Id);
        updatedWallet.Should().NotBeNull();
        updatedWallet!.Assets.Should().ContainSingle(s => 
            s.Symbol == command.Symbol && 
            s.PurchaseDate.Date == command.PurchaseDate.Date &&
            s.Price == command.Price && s.Quantity == command.Quantity
            );
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenWalletDoesNotExist()
    {
        // Arrange
        var walletId = WalletId.CreateUnique();
        var command = new RecordAssetTransaction(
            walletId,
            "MSFT",
            DateTime.UtcNow,
            300.75m,
            3
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain(walletId.ToString());
    }
}
