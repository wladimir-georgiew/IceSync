namespace UniversalLoaderClient.Contracts;

public interface IUniLoaderClientBuilder
{
    public IUniLoaderClient Build(string? authToken = "");
}