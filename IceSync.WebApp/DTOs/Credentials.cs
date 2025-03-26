namespace IceSync.WebApp.DTOs;

public record Credentials
{
    public required string CompanyId { get; init; }
    public required string UserId { get; init; }
    public required string Secret { get; init; }
}