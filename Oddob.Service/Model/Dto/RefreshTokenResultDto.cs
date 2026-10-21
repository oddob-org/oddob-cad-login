namespace Oddob.Service.Model.Dto;

public class RefreshTokenResultDto
{
    [JsonProperty("loginResultType")]
    public AccountResultType LoginResultType { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; } = "";

    [JsonProperty("expire")]
    public long Expire { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = "";
}