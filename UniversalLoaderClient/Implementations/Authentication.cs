using IceSync.Common.Contracts;
using IceSync.Common.Contracts.UniLoader.AuthV2;
using IceSync.Common.Implementations;

namespace UniversalLoaderClient.Implementations;

public class Authentication(Requester requester)
{
    public async Task<BaseResult<AuthV2Response>> AuthenticateAsync(AuthV2Request request)
    {
        string url = $"v2/authenticate";
        return await requester.SendRequestAsync<AuthV2Response>(HttpMethod.Post, url, request);
    }
}