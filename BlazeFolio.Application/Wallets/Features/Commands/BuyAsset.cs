using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.Providers;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record BuyAsset(
    WalletId WalletId,
    string Symbol,
    DateTime PurchaseDate,
    decimal Price,
    int Quantity) : IRequest<Result>;

public class RecordAssetTransactionHandler : IRequestHandler<BuyAsset, Result>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IMarketMetadataProvider _metadataProvider;

    public RecordAssetTransactionHandler(IWalletRepository walletRepository, IMarketMetadataProvider metadataProvider)
    {
        _walletRepository = walletRepository;
        _metadataProvider = metadataProvider;
    }

    public async Task<Result> Handle(BuyAsset request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure($"Wallet with id: {request.WalletId} not found.");
        }

        var asset = Asset.Create(request.Symbol, request.PurchaseDate, request.Quantity, request.Price);
        wallet.AddAsset(asset);

        // Fetch metadata for the asset and store it
        var metadata = await _metadataProvider.GetMetadataAsync(request.Symbol);
        if (metadata != null)
        {
            // Update asset with metadata
            asset.UpdateMetadata(metadata);
        }

        await _walletRepository.UpdateAsync(wallet);

        return Result.Success();
    }
}