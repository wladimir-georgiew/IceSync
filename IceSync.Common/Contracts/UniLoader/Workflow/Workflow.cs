using Newtonsoft.Json;

namespace IceSync.Common.Contracts.UniLoader.Workflow;

public record Workflow
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("version")]
    public int Version { get; set; }

    [JsonProperty("versionName")]
    public string VersionName { get; set; }

    [JsonProperty("versionNotes")]
    public string VersionNotes { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("isActive")]
    public bool IsActive { get; set; }

    [JsonProperty("creationDateTime")]
    public DateTime CreationDateTime { get; set; }

    [JsonProperty("creationUserId")]
    public int CreationUserId { get; set; }

    [JsonProperty("ownerUserId")]
    public int OwnerUserId { get; set; }

    [JsonProperty("multiExecBehavior")]
    public string MultiExecBehavior { get; set; }

    [JsonProperty("executionRetriesCount")]
    public int? ExecutionRetriesCount { get; set; }

    [JsonProperty("executionRetriesPeriod")]
    public int? ExecutionRetriesPeriod { get; set; }

    [JsonProperty("executionRetriesPeriodTimeUnit")]
    public string ExecutionRetriesPeriodTimeUnit { get; set; }

    [JsonProperty("workflowGroupId")]
    public int WorkflowGroupId { get; set; }

    [JsonProperty("canStoreSuccessExecutionData")]
    public bool CanStoreSuccessExecutionData { get; set; }

    [JsonProperty("successExecutionDataRetentionPeriodDays")]
    public int? SuccessExecutionDataRetentionPeriodDays { get; set; }

    [JsonProperty("canStoreWarningExecutionData")]
    public bool CanStoreWarningExecutionData { get; set; }

    [JsonProperty("warningExecutionDataRetentionPeriodDays")]
    public int? WarningExecutionDataRetentionPeriodDays { get; set; }

    [JsonProperty("canStoreFailureExecutionData")]
    public bool CanStoreFailureExecutionData { get; set; }

    [JsonProperty("failureExecutionDataRetentionPeriodDays")]
    public int? FailureExecutionDataRetentionPeriodDays { get; set; }

    [JsonProperty("connectorLogLevel")]
    public string ConnectorLogLevel { get; set; }
}