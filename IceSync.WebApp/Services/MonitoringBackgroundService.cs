﻿using IceSync.Domain;
using IceSync.WebApp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UniversalLoaderClient.Contracts;

namespace IceSync.WebApp.Services;

public class MonitoringBackgroundService(ILogger<MonitoringBackgroundService> logger, IServiceProvider serviceProvider,
    IConfigurationService configurationService, IUniLoaderClientBuilder clientBuilder, IAuthCacheService authCacheService)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await StartAsync(stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "{Name} stopped unexpectedly, due to error.", nameof(MonitoringBackgroundService));
            throw;
        }
    }

    private async Task StartAsync(CancellationToken stoppingToken)
    {
        var configuration = configurationService.GetConfiguration();

        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = serviceProvider.CreateScope();
            var authBearerToken = await authCacheService.GetBearerTokenAsync();
            
            var client = clientBuilder.Build(authBearerToken);
            var dbContext = scope.ServiceProvider.GetRequiredService<IceSyncDbContext>();
            
            // TODO: 
            // If there is a new workflow returned from the API which is not present in the database it should insert it
            // If a workflow is not returned from the API but is present in the database it should be deleted from the database
            // For all other workflows update the fields in the database with the values returned from the API
            // The unique identifier for the workflows is their Id
            scope.Dispose();
            
            await Task.Delay(configuration.MonitoringTriggerTime ,stoppingToken);
        }
    }
}