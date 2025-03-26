using IceSync.Common.Implementations;
using UniversalLoaderClient.Contracts;
using UniversalLoaderClient.Implementations;

namespace UniversalLoaderClient;

public class UniLoaderClient : IUniLoaderClient
{
    private readonly Lazy<Authentication> _authentication;
    private readonly Lazy<Workflows> _workflows;
    
    public UniLoaderClient(Requester requester)
    {
        _authentication = new Lazy<Authentication>(() => new Authentication(requester));
        _workflows = new Lazy<Workflows>(() => new Workflows(requester));
    }

    public Authentication Authentication => _authentication.Value;
    public Workflows Workflows => _workflows.Value;
}