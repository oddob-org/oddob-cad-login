namespace Oddob.Service.Constant;

public static class ApiConstant
{
    /// <summary>
    /// 获取图形验证码
    /// </summary>
    public const string GetPngVerificationCode = "/api/open/verifyCode";

    /// <summary>
    /// 获取手机验证码
    /// </summary>
    public const string GetPhoneVerificationCode = "/api/open/getPhoneCode";

    /// <summary>
    /// 登录
    /// </summary>
    public const string Login = "/api/open/login";

    /// <summary>
    /// 刷新token
    /// </summary>
    public const string RefreshToken = "/api/open/getToken";

    /// <summary>
    /// 获取用户信息
    /// </summary>
    public const string GetUserInfo = "/api/open/userInfo";
}