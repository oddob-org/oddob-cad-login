namespace Oddob.Service.Service.Interface;

/// <summary>
/// 用户设置
/// </summary>
internal interface IUserSettingService
{
    /// <summary>
    /// 重新加载用户设置
    /// </summary>
    /// <param name="userSetting">用户设置</param>
    /// <returns>如果成功则返回<c>true</c>，否则返回<c>false</c></returns>
    bool Reload(IUserSetting userSetting);

    /// <summary>
    /// 保存用户设置
    /// </summary>
    /// <param name="userSetting">用户设置</param>
    /// <returns>如果成功则返回<c>true</c>，否则返回<c>false</c></returns>
    bool Save(IUserSetting userSetting);

    /// <summary>
    /// 读取所有用户设置
    /// </summary>
    void ReloadAll();
}

/// <summary>
/// 用户设置接口
/// </summary>
public interface IUserSetting;