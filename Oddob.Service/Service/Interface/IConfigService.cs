namespace Oddob.Service.Service.Interface;

internal interface IConfigService
{
    /// <summary>
    /// 读取
    /// </summary>
    void Reload();

    /// <summary>
    /// 获取Url
    /// </summary>
    /// <returns>服务器地址</returns>
    string GetUrl();

    /// <summary>
    /// 获取ApiString
    /// </summary>
    string GetApiString(string key);
}