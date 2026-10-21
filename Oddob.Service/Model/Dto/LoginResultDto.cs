namespace Oddob.Service.Model.Dto;

public class LoginResultDto
{
    [JsonProperty("loginResultType")]
    public AccountResultType AccountResultType { get; set; } = AccountResultType.ERROR;

    [JsonProperty("token")]
    public string Token { get; set; } = "";

    [JsonProperty("userInfo")]
    public UserInfo UserInfo { get; set; } = new();

    [JsonProperty("message")]
    public string Message { get; set; } = "";
}

public class FlushTokenDto
{
    [JsonProperty("loginResultType")]
    public AccountResultType AccountResultType { get; set; } = AccountResultType.ERROR;

    [JsonProperty("token")]
    public string Token { get; set; } = "";

    [JsonProperty("exprie")]
    public DateTime Exprie { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; } = "";
}

public enum AccountResultType : byte
{
    [Description("错误")]
    ERROR = 0,

    [Description("登录失败")]
    FAIL,

    [Description("登录成功")]
    SUCCESS,

    [Description("需要验证码")]
    NEED_VERIFICATION_CODE,
}