using Newtonsoft.Json;

namespace IceSync.Common.Contracts.UniLoader.Workflow;

public record Workflow
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("isActive")]
    public bool IsActive { get; set; }

    [JsonProperty("multiExecBehavior")]
    public string MultiExecBehavior { get; set; }
}