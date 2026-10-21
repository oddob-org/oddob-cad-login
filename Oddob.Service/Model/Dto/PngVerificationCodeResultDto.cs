namespace Oddob.Service.Model.Dto;

/// <summary>
/// 图形验证码结果Dto
/// </summary>
public class PngVerificationCodeResultDto
{
    [JsonProperty("verifyCode")]
    public string VerifyCode { get; set; } = "";

    [JsonProperty("verifyCodeTime")]
    public string VerifyCodeTime { get; set; } = "";

    [JsonProperty("verifyCodeHash")]
    public string VerifyCodeHash { get; set; } = "";

    [JsonProperty("message")]
    public string Message { get; set; } = "";
}