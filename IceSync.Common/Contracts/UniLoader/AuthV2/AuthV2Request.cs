namespace IceSync.Common.Contracts.UniLoader.AuthV2;

public record AuthV2Request
{
    public string ApiCompanyId { get; set; }
    public string ApiUserId { get; set; }
    public string ApiUserSecret { get; set; }
}