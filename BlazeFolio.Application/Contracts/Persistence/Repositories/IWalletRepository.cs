

using BlazeFolio.Domain.WalletAggregate;
using BlazeFolio.Domain.WalletAggregate.ValueObjects;

namespace BlazeFolio.Application.Contracts.Persistence.Repositories;

public interface IWalletRepository
{
    Task<Wallet?> GetByIdAsync(WalletId id);
    Task<List<Wallet>> GetAllAsync();
    Task<Wallet> AddAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
    Task DeleteAsync(WalletId id);
}