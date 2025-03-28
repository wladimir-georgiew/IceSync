using IceSync.Common.Contracts.UniLoader.AuthV2;
using IceSync.WebApp.Contracts;
using IceSync.WebApp.DTOs;
using Microsoft.Extensions.Logging;
using UniversalLoaderClient.Contracts;

namespace IceSync.WebApp.Services;

public class AuthCacheService : IAuthCacheService
{
    private readonly ILogger<AuthCacheService> _logger;
    private readonly IUniLoaderClientBuilder _uniLoaderClientBuilder;
    private readonly IConfigurationService _configurationService;
    
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private string _token;
    private DateTime _expiresAt;

    public AuthCacheService(ILogger<AuthCacheService> logger, IUniLoaderClientBuilder uniLoaderClientBuilder, IConfigurationService configurationService)
    {
        _logger = logger;
        _uniLoaderClientBuilder = uniLoaderClientBuilder;
        _configurationService = configurationService;
    }

    public async Task<string> GetBearerTokenAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (_token == null || DateTime.UtcNow >= _expiresAt)
            {
                _logger.LogInformation("AccessToken missing or expired, attempting to get new one..");
                await RefreshTokenAsync();
            }
                
            _logger.LogInformation("Successfully retrieved AccessToken. Expires at {ExpiresAt}.", _expiresAt);
            return _token;
        }
        finally
        {
            _semaphore.Release();
        }
    }
    
    private async Task RefreshTokenAsync()
    {
        var authResult = await AuthenticateAsync(_configurationService.GetConfiguration().Credentials);

        _token = authResult.BearerToken;
        _expiresAt = DateTime.UtcNow.AddSeconds(authResult.ExpiresIn - 20); // Refresh a bit earllier
    }

    private async Task<AuthenticationResult> AuthenticateAsync(Credentials credentials)
    {
        var uniLoaderClient = _uniLoaderClientBuilder.Build();
        var authRequest = new AuthV2Request()
        {
            ApiCompanyId = credentials.CompanyId,
            ApiUserId = credentials.UserId,
            ApiUserSecret = credentials.Secret,
        };
                
        var authResponse = await uniLoaderClient.Authentication.AuthenticateAsync(authRequest);

        if (!authResponse.Success)
        {
            throw new Exception($"Auth failed - Code: {authResponse.Error.Code}, MSG: {authResponse.Error.Message}");
        }

        return new AuthenticationResult
        {
            BearerToken = authResponse.Data.TokenType + " " + authResponse.Data.AccessToken,
            ExpiresIn = authResponse.Data.ExpiresIn,
        };
    }
}