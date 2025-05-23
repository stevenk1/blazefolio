using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Commands;

public partial class AddWalletCommand : IRequest<Result<Guid>>
{
    public string Name { get; set; }
    public byte[] Picture { get; set; }
    public WalletType Type { get; set; }
}

public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, Result<Guid>>
{
    private readonly IWalletRepository _walletRepository;

    public AddWalletCommandHandler(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<Result<Guid>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = Wallet.CreateNew(request.Name, request.Picture, request.Type);

        await _walletRepository.AddAsync(wallet);

        return Result.Success(wallet.Id.Value);
    }
}