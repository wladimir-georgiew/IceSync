using Newtonsoft.Json;

namespace IceSync.Common.Contracts.UniLoader.Workflow;

public record WorkflowRunRequest
{
    public string WaitOutput { get; set; }
    public string DecodeOutputJsonString { get; set; }
}