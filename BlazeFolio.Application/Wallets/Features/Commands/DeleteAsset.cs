using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record DeleteAsset(
    WalletId WalletId,
    AssetId AssetId) : IRequest<Result>;

public class DeleteAssetHandler : IRequestHandler<DeleteAsset, Result>
{
    private readonly IWalletRepository _walletRepository;

    public DeleteAssetHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(DeleteAsset request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure($"Wallet with id: {request.WalletId} not found.");
        }

        var asset = wallet.Assets.FirstOrDefault(a => a.Id == request.AssetId);

        if (asset is null)
        {
            return Result.Failure($"Asset with id: {request.AssetId} not found in wallet.");
        }

        wallet.RemoveAsset(asset);
        await _walletRepository.UpdateAsync(wallet);

        return Result.Success();
    }
}
