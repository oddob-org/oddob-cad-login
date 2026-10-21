namespace Oddob.Service.Model.Settings;

/// <summary>
/// 登录设置
/// </summary>
[SingletonDI]
public class AccountSettings : IUserSetting
{
    /// <summary>
    /// 账号
    /// </summary>
    public string Mail { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// 记住密码
    /// </summary>
    public bool RememberPassword { get; set; }
}