namespace Oddob.Service.Constant;

public enum AccountPageType : byte
{
    [Description("登录")]
    LOGIN = 0,

    // [Description("注册")]
    // REGISTER,
    //
    // [Description("重置密码")]
    // RESET_PASSWORD,

    [Description("用户信息")]
    USER_INFO,
}