namespace IceSync.WebApp.DTOs;

public record AuthenticationResult
{
    public string BearerToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}