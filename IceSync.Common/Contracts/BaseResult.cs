namespace IceSync.Common.Contracts;

public record BaseResult
{
    public bool Success { get; init; }
    public Error? Error { get; init; }
}

public record BaseResult<T>
{
    public bool Success { get; init; }
    public T? Data { get; init; }
    public Error? Error { get; init; }
}

public record Error
{
    public int Code { get; init; }
    public string Message { get; init; } = "Unknown error";
}

