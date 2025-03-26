using IceSync.Common.Contracts.UniLoader.AuthV2;
using IceSync.WebApp.Contracts;
using IceSync.WebApp.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniversalLoaderClient.Contracts;

namespace IceSync.WebApp.Services;

public class MonitoringBackgroundService(IServiceProvider serviceProvider, IConfigurationService configurationService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var configuration = configurationService.GetConfiguration();
        var authBearerToken = string.Empty;
        var authExpiresAt = DateTime.UtcNow;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var scope = serviceProvider.CreateScope();
            var clientBuilder = scope.ServiceProvider.GetRequiredService<IUniLoaderClientBuilder>();

            // Needs authenticating
            if (string.IsNullOrWhiteSpace(authBearerToken) || DateTime.UtcNow > authExpiresAt - TimeSpan.FromSeconds(10))
            {
                var authResult = await AuthenticateAsync(clientBuilder, configuration.Credentials);
                authBearerToken = authResult.BearerToken;
                authExpiresAt = authResult.ExpiresAt;
            }
            
            var client = clientBuilder.Build(authBearerToken);
            
            // TODO: 
            // If there is a new workflow returned from the API which is not present in the database it should insert it
            // If a workflow is not returned from the API but is present in the database it should be deleted from the database
            // For all other workflows update the fields in the database with the values returned from the API
            // The unique identifier for the workflows is their Id
            
            await Task.Delay(configuration.MonitoringTriggerTime ,stoppingToken);
        }
    }

    private async Task<AuthenticationResult> AuthenticateAsync(IUniLoaderClientBuilder clientBuilder, Credentials credentials)
    {
        var client = clientBuilder.Build();
        var authRequest = new AuthV2Request()
        {
            ApiCompanyId = credentials.CompanyId,
            ApiUserId = credentials.UserId,
            ApiUserSecret = credentials.Secret,
        };
                
        var authResponse = await client.Authentication.AuthenticateAsync(authRequest);

        if (!authResponse.Success)
        {
            throw new Exception($"Auth failed - Code: {authResponse.Error.Code}, MSG: {authResponse.Error.Message}");
        }

        return new AuthenticationResult
        {
            BearerToken = authResponse.Data.TokenType + " " + authResponse.Data.AccessToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(authResponse.Data.ExpiresIn)
        };
    }
}