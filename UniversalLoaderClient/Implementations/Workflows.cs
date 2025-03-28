using IceSync.Common.Contracts;
using IceSync.Common.Contracts.UniLoader.Workflow;
using IceSync.Common.Implementations;

namespace UniversalLoaderClient.Implementations;

public class Workflows(Requester requester)
{
    public async Task<BaseResult<List<Workflow>>> AllAsync()
    {
        string url = $"workflows";
        return await requester.SendRequestAsync<List<Workflow>>(HttpMethod.Get, url);
    }
    
    public async Task<BaseResult> RunAsync(WorkflowRunRequest request)
    {
        string url = $"workflows/{request.WorkflowId}/run";
        return await requester.SendRequestAsync(HttpMethod.Post, url, request);
    }
}