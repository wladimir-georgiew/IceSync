namespace IceSync.WebApp.DTOs;

public record AuthenticationResult
{
    public string BearerToken { get; init; }
    public int ExpiresIn { get; init; }
}