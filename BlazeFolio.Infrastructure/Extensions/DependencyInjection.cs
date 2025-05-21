using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Infrastructure.Persistence;
using BlazeFolio.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazeFolio.Infrastructure.Extensions;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Register LiteDB context
        services.AddSingleton(_ => new LiteDbContext(connectionString));
        
        // Register repositories
        services.AddScoped<IWalletRepository, WalletRepository>();
    }
}
