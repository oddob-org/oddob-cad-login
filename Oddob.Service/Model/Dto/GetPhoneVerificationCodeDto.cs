namespace Oddob.Service.Model.Dto;

public class GetPhoneVerificationCodeDto
{
    [JsonProperty("accountNumber")]
    public string AccountNumber { get; set; } = "";

    [JsonProperty("vercode")]
    public string VerificationCode { get; set; } = "";

    [JsonProperty("time")]
    public string Time { get; set; } = "";

    [JsonProperty("hash")]
    public string Hash { get; set; } = "";
}