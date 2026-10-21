namespace Oddob.Service.Service.Interface.Account;

internal interface IAccountService
{
    // /// <summary>
    // /// 已登录
    // /// </summary>
    // /// <returns>是则返回<c>true</c></returns>
    // bool IsLogin { get; }

    /// <summary>
    /// 获取验证码图片
    /// </summary>
    Task<ResponseBody<PngVerificationCodeResultDto>> GetPngVerificationCode();

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <param name="accountNumber">邮箱</param>
    /// <param name="pngCode">图片验证码</param>
    /// <param name="pngInfo">图片验证码信息</param>
    /// <returns>获取情况</returns>
    Task<string> GetPhoneVerificationCode(string accountNumber,string pngCode, PngVerificationCodeResultDto  pngInfo);

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="accountNumber">账号</param>
    /// <param name="password">密码</param>
    /// <param name="verificationCode">验证码</param>
    /// <returns></returns>
    Task<ResponseBody<LoginResultDto>> Login(string accountNumber, string password, string verificationCode);

    /// <summary>
    /// 静默登录
    /// </summary>
    /// <returns>成功返回<c>true</c></returns>
    Task<bool> QuiteLoginAsync();

    /// <summary>
    /// 静默登录
    /// </summary>
    /// <returns>成功返回<c>true</c></returns>
    bool QuiteLogin();

    /// <summary>
    /// 退出登录方法
    /// </summary>
    void Logout();


    /// <summary>
    /// 保存设置
    /// </summary>
    void SaveAccountSettings();

    // /// <summary>
    // /// 读取设置
    // /// </summary>
    // void ReloadAccountSettings();
}