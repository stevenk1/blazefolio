using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;
using BlazeFolio.Infrastructure.Persistence;
using BlazeFolio.Infrastructure.Persistence.Models;

namespace BlazeFolio.Infrastructure.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly LiteDbContext _dbContext;

        public WalletRepository(LiteDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Wallet?> GetByIdAsync(WalletId id)
        {
            var walletModel = _dbContext.Wallets.FindById(id.Value);
            return Task.FromResult(walletModel?.ToDomain());
        }

        public Task<List<Wallet>> GetAllAsync()
        {
            var walletModels = _dbContext.Wallets.FindAll().ToList();
            return Task.FromResult(walletModels.Select(w => w.ToDomain()).ToList());
        }

        public Task<Wallet> AddAsync(Wallet wallet)
        {
            var walletModel = WalletModel.FromDomain(wallet);
            _dbContext.Wallets.Insert(walletModel);
            return Task.FromResult(wallet);
        }

        public Task UpdateAsync(Wallet wallet)
        {
            var walletModel = WalletModel.FromDomain(wallet);
            _dbContext.Wallets.Update(walletModel);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(WalletId id)
        {
            _dbContext.Wallets.Delete(id.Value);
            return Task.CompletedTask;
        }
    }
}