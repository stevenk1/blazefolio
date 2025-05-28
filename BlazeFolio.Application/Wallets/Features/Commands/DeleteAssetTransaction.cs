using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record DeleteAssetTransaction(
    WalletId WalletId,
    string Symbol) : IRequest<Result>;

public class DeleteAssetTransactionHandler : IRequestHandler<DeleteAssetTransaction, Result>
{
    private readonly IWalletRepository _walletRepository;

    public DeleteAssetTransactionHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(DeleteAssetTransaction request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure($"Wallet with id: {request.WalletId} not found.");
        }

        var asset = wallet.Assets.FirstOrDefault(a => a.Symbol == request.Symbol);

        if (asset is null)
        {
            return Result.Failure($"Asset with symbol: {request.Symbol} not found in wallet.");
        }

        wallet.RemoveAsset(asset);
        await _walletRepository.UpdateAsync(wallet);

        return Result.Success();
    }
}
