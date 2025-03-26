namespace IceSync.WebApp.Contracts;

public interface IAuthCacheService
{
    public Task<string> GetBearerTokenAsync();
}