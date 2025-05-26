using BlazeFolio.Application.Contracts.Infrastructure;
using BlazeFolio.Application.Contracts.Persistence.Repositories;
using BlazeFolio.Infrastructure.Persistence;
using BlazeFolio.Infrastructure.Repositories;
using BlazeFolio.Infrastructure.Services;
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

        // Register infrastructure services
        services.AddSingleton<IFusionCache, FusionCache>();
    }
}
