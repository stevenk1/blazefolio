using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Queries;

public record GetWalletById(WalletId WalletId) : IRequest<Result<Wallet>>;

public class GetWalletByIdHandler : IRequestHandler<GetWalletById, Result<Wallet>>
{
    private readonly IWalletRepository _walletRepository;

    public GetWalletByIdHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result<Wallet>> Handle(GetWalletById request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);

        if (wallet is null)
        {
            return Result.Failure<Wallet>($"Wallet with id: {request.WalletId} not found.");
        }

        return Result.Success(wallet);
    }
}
