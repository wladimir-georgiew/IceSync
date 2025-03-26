using IceSync.Common;
using IceSync.WebApp.Contracts;
using IceSync.WebApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversalLoaderClient;
using UniversalLoaderClient.Contracts;
using UniversalLoaderClient.Services;

namespace IceSync.WebApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Client Registrations
        RegisterUniLoaderClientBuilder(services, configuration);
        RegisterDatabase(services, configuration);
        
        // Service Registrations
        services.AddHostedService<MonitoringBackgroundService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IAuthCacheService, AuthCacheService>();
        
        return services;
    }
    
    private static void RegisterDatabase(IServiceCollection services, IConfiguration  configuration)
    {
        
    }

    private static void RegisterUniLoaderClientBuilder(IServiceCollection services, IConfiguration  configuration)
    {
        var uniLoaderUrl = configuration.GetSection("Services").GetSection("UniversalLoader").GetValue<string>("Url");
        services.AddHttpClient(HttpClientNameConstants.UniLoaderApi, client =>
        {
            client.BaseAddress = new Uri(uniLoaderUrl);
        });
        services.AddSingleton<IUniLoaderClientBuilder, UniLoaderClientBuilder>();
    }
}