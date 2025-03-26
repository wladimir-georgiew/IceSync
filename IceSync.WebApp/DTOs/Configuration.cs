namespace IceSync.WebApp.DTOs;

public record Configuration
{
    public required TimeSpan MonitoringTriggerTime { get; init; }
    public required Credentials Credentials { get; init; }
}