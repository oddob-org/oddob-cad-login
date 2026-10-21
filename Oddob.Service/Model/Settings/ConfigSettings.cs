namespace Oddob.Service.Model.Settings;

/// <summary>
/// 配置类
/// 用于存储网络连接的配置信息，如主机地址和端口号
/// </summary>
[SingletonDI]
public class ConfigSettings
{
    /// <summary>
    /// 主机地址
    /// 默认值为空字符串，需要在使用前设置正确的主机地址
    /// </summary>
    public string Host { get; set; } = "https://oddob.com";

    /// <summary>
    /// 端口号
    /// 默认值为空字符串，需要在使用前设置正确的端口号
    /// </summary>
    public string Port { get; set; } = "";
}