using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.Common;
using BlazeFolio.Domain.WalletAggregate.Exceptions;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record SellAsset(
    WalletId WalletId,
    AssetId AssetId,
    DateTime SaleDate,
    decimal Price,
    int Quantity) : IRequest<Result>;

public class SellAssetHandler : IRequestHandler<SellAsset, Result>
{
    private readonly IWalletRepository _walletRepository;

    public SellAssetHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(SellAsset request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure($"Wallet with id: {request.WalletId} not found.");
        }

        try
        {
            // Use the domain method to sell the asset
            wallet.SellAsset(
                request.AssetId,
                request.Quantity,
                request.SaleDate,
                request.Price);

            await _walletRepository.UpdateAsync(wallet);
            return Result.Success();
        }
        catch (AssetNotFoundException ex)
        {
            return Result.Failure($"Asset not found in wallet: {ex.Message}");
        }
        catch (InsufficientSharesException ex)
        {
            return Result.Failure($"Insufficient shares: {ex.Message}");
        }
        catch (DomainException ex)
        {
            return Result.Failure($"Domain error: {ex.Message}");
        }
    }
}
