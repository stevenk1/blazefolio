using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using CSharpFunctionalExtensions;
using MediatR;

namespace BlazeFolio.Application.Wallets.Features.Queries;
public record GetAllWallets() : IRequest<Result<List<Wallet>>>;


    public class GetAllWalletsHandler : IRequestHandler<GetAllWallets, Result<List<Wallet>>>
    {
        private readonly IWalletRepository _walletRepository;

        public GetAllWalletsHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Result<List<Wallet>>> Handle(GetAllWallets request, CancellationToken cancellationToken)
        {
            var wallets = await _walletRepository.GetAllAsync();
            return Result.Success(wallets);
        }
    
}