using UniversalLoaderClient.Implementations;

namespace UniversalLoaderClient.Contracts;

public interface IUniLoaderClient
{
    Authentication Authentication { get; }
    Workflows Workflows { get; }
}