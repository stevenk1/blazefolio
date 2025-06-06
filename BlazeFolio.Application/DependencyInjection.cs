using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BlazeFolio.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
       
        return services;
    }
}
