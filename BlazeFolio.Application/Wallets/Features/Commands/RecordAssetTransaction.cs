using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.Entities;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record RecordAssetTransaction(
    WalletId WalletId,
    string Symbol,
    DateTime PurchaseDate,
    decimal Price,
    int Quantity) : IRequest<Result>;

public class RecordAssetTransactionHandler : IRequestHandler<RecordAssetTransaction, Result>
{
    private readonly IWalletRepository _walletRepository;

    public RecordAssetTransactionHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(RecordAssetTransaction request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure($"Wallet with id: {request.WalletId} not found.");
        }

        var asset = Asset.Create(request.Symbol, request.PurchaseDate, request.Quantity, request.Price);
        wallet.AddAsset(asset);

        await _walletRepository.UpdateAsync(wallet);

        return Result.Success();
    }
}