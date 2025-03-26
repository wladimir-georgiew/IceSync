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
        AddUniLoaderClientBuilder(services, configuration);
        
        // Service Registrations
        services.AddHostedService<MonitoringBackgroundService>();
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        
        return services;
    }

    private static void AddUniLoaderClientBuilder(IServiceCollection services, IConfiguration  configuration)
    {
        var uniLoaderUrl = configuration.GetSection("Services").GetSection("UniversalLoader").GetValue<string>("Url");
        services.AddHttpClient(HttpClientNameConstants.UniLoaderApi, client =>
        {
            client.BaseAddress = new Uri(uniLoaderUrl);
        });
        services.AddScoped<IUniLoaderClientBuilder, UniLoaderClientBuilder>();
    }
}