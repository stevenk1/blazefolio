using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public record DeleteWalletCommand(WalletId WalletId) : IRequest<Result>;

public class DeleteWalletCommandHandler : IRequestHandler<DeleteWalletCommand, Result>
{
    private readonly IWalletRepository _walletRepository;

    public DeleteWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _walletRepository.DeleteAsync(request.WalletId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete wallet: {ex.Message}");
        }
    }
}
