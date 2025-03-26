using IceSync.Common;
using IceSync.Common.Implementations;
using UniversalLoaderClient.Contracts;

namespace UniversalLoaderClient.Services;

public class UniLoaderClientBuilder(IHttpClientFactory factory) : IUniLoaderClientBuilder
{
    public IUniLoaderClient Build(string? authToken = "")
    {
        HttpClient client = factory.CreateClient(HttpClientNameConstants.UniLoaderApi);
        
        if (!string.IsNullOrWhiteSpace(authToken))
        {
            client.DefaultRequestHeaders.Add("Authorization", authToken);
        }
        
        var requester = new Requester(client);
        return new UniLoaderClient(requester);
    }
}