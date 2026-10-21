namespace Oddob.Service.Model.Dto;

/// <summary>
/// 用户登录数据模型(接收)
/// </summary>
public class UserLoginDto
{
    /// <summary>
    /// 名字
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = "";

    /// <summary>
    /// 哈希值
    /// </summary>
    public string HashCode { get; set; } = "";

    /// <summary>
    /// 验证码
    /// </summary>
    public string VerificationCode { get; set; } = "";
}