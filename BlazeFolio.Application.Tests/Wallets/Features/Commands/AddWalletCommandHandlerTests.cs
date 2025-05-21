using BlazeFolio.Application.Tests.Testing;
using BlazeFolio.Application.Wallets.Features.Commands;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazeFolio.Application.Tests.Wallets.Features.Commands;

public class AddWalletCommandHandlerTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;
    
    public AddWalletCommandHandlerTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Fact]
    public async Task Handle_ShouldCreateNewWallet_AndReturnItsId()
    {
        // Arrange
        var handler = new AddWalletCommandHandler(_fixture.WalletRepository);
        var command = new AddWalletCommand
        {
            Name = "Test Wallet",
            Picture = [1, 2, 3, 4]
        };
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        
        // Verify wallet was saved to the database
        var savedWallet = await _fixture.WalletRepository.GetByIdAsync(
            Domain.WalletAggregate.ValueObjects.WalletId.Create(result.Value));
        
        savedWallet.Should().NotBeNull();
        savedWallet!.Name.Should().Be(command.Name);
        savedWallet.Picture.Value.Should().BeEquivalentTo(command.Picture);
    }
}
