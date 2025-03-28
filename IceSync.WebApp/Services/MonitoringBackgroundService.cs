using IceSync.Common.Contracts.UniLoader.Workflow;
using IceSync.Domain;
using IceSync.Domain.Entities;
using IceSync.WebApp.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UniversalLoaderClient.Contracts;

namespace IceSync.WebApp.Services;

public class MonitoringBackgroundService(
    ILogger<MonitoringBackgroundService> logger,
    IServiceProvider serviceProvider,
    IConfigurationService configurationService,
    IUniLoaderClientBuilder clientBuilder,
    IAuthCacheService authCacheService)
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
            var uniLoaderWorkflowsResponse = await client.Workflows.AllAsync();

            if (!uniLoaderWorkflowsResponse.Success || uniLoaderWorkflowsResponse.Data is null)
            {
                logger.LogError(
                    "Failed to fetch workflows from UniLoader api.. Reason: {Message}, Code: {Code}. Try again in {TriggerTime}",
                    uniLoaderWorkflowsResponse.Error.Message, uniLoaderWorkflowsResponse.Error.Code, configuration.MonitoringTriggerTime);

                await Task.Delay(configuration.MonitoringTriggerTime, stoppingToken);
                return;
            }

            var dbContext = scope.ServiceProvider.GetRequiredService<IceSyncDbContext>();
            var dbWorkflows = dbContext.Workflows.ToList();

            await SynchronizeDatabaseWithApi(uniLoaderWorkflowsResponse.Data, dbWorkflows, dbContext, stoppingToken);

            scope.Dispose();
            
            logger.LogInformation("Synchronization completed, next one triggers in {waitTime} ..", configuration.MonitoringTriggerTime);
            await Task.Delay(configuration.MonitoringTriggerTime, stoppingToken);
        }
    }

    private async Task SynchronizeDatabaseWithApi(List<Workflow> apiWorkflows, List<WorkflowEntity> dbWorkflows,
        IceSyncDbContext dbContext, CancellationToken stoppingToken)
    {
        foreach (var apiWorkflow in apiWorkflows)
        {
            var existingWorkflowEntity = dbWorkflows.FirstOrDefault(x => x.WorkflowId == apiWorkflow.Id);
                
            // 1. If there is a new workflow returned from the API which is not present in the database it should insert it
            if (existingWorkflowEntity is null)
            {
                var newWorkflow = new WorkflowEntity()
                {
                    WorkflowId = apiWorkflow.Id,
                    WorkflowName = apiWorkflow.Name,
                    IsActive = apiWorkflow.IsActive,
                    MultiExecBehavior = apiWorkflow.MultiExecBehavior,
                };
                dbContext.Add(newWorkflow);
                    
                logger.LogInformation("ADD workflow - {entity}", JsonConvert.SerializeObject(newWorkflow));
            }
            // 3. For all other workflows update the fields in the database with the values returned from the API
            else
            {
                existingWorkflowEntity.WorkflowName = apiWorkflow.Name;
                existingWorkflowEntity.IsActive = apiWorkflow.IsActive;
                existingWorkflowEntity.MultiExecBehavior = apiWorkflow.MultiExecBehavior;

                logger.LogInformation("UPDATE workflow - {entity}, with new values: Name: {Name}, IsActive: {IsActive}, MultiExecBehavior: {MultiExec}",
                    JsonConvert.SerializeObject(existingWorkflowEntity), apiWorkflow.Name, apiWorkflow.IsActive, apiWorkflow.MultiExecBehavior);
            }
        }
            
        // 2. If a workflow is not returned from the API but is present in the database it should be deleted from the database
        var apiWorkflowIds = new HashSet<int>(apiWorkflows.Select(x => x.Id));
        var workflowsToRemove = dbWorkflows.Where(dbWorkflow => !apiWorkflowIds.Contains(dbWorkflow.WorkflowId)).ToList();

        if (workflowsToRemove.Any())
        {
            dbContext.Workflows.RemoveRange(workflowsToRemove);
                
            logger.LogInformation("DELETE workflows with the following IDS - {workflows}", workflowsToRemove.Select(x => x.WorkflowId));
        }
            
        await dbContext.SaveChangesAsync(stoppingToken);
    }
}