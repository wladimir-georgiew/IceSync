using Newtonsoft.Json;

namespace IceSync.Common.Contracts.UniLoader.Workflow;

public record WorkflowRunRequest
{
    public string WorkflowId { get; set; }
    public string WaitOutput { get; set; }
    public string DecodeOutputJsonString { get; set; }
}