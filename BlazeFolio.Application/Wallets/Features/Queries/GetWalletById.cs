using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Queries;

public record GetWalletById(WalletId WalletId) : IRequest<Result<WalletDto>>;

public class GetWalletByIdHandler : IRequestHandler<GetWalletById, Result<WalletDto>>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletByIdHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result<WalletDto>> Handle(GetWalletById request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure<WalletDto>($"Wallet with id: {request.WalletId} not found.");
        }

        var walletDto = new WalletDto
        {
            Id = wallet.Id.Value,
            Name = wallet.Name,
            Balance = 0, // todo
            Assets = wallet.Assets.Select(a => new AssetDto
            {
                Id = a.Id.Value,
                Symbol = a.Symbol,
                PurchaseDate = a.PurchaseDate,
                Quantity = a.Quantity,
                PurchasePrice = a.Price,
            }).ToList()
        };

        return Result.Success(walletDto);
    }
}

public class WalletDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public List<AssetDto> Assets { get; set; } = new();
}

public class AssetDto
{
    public Guid Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }

    // Calculated properties (would typically be populated from a market data service)
    public string CompanyName { get; set; } = string.Empty;
    public decimal CurrentPrice { get; set; }
    public decimal MarketValue => CurrentPrice * Quantity;
    public decimal ProfitLossAmount => MarketValue - (PurchasePrice * Quantity);
    public decimal ProfitLossPercentage => PurchasePrice > 0 ? (CurrentPrice - PurchasePrice) / PurchasePrice * 100 : 0;
}
