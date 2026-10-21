namespace Oddob.Service.Model.Dto;

public class PhoneVerifyCodeResultDto
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    // [JsonProperty("code")]
    // public int Code { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = "";
}