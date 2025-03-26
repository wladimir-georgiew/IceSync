namespace IceSync.Common.Contracts;

public record BaseResponse
{
    public string? Error { get; init; }
}