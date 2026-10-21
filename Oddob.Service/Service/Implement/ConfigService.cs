using Oddob.Service.Model.Settings;

namespace Oddob.Service.Service.Implement;

/// <summary>
/// 配置服务
/// </summary>
[SingletonDI(typeof(IConfigService))]
internal sealed class ConfigService : IConfigService
{
    private readonly ConfigSettings _configSettings;
    private readonly string _configFilePath;
    private readonly IJsonService _jsonService;

    public ConfigService(ConfigSettings configSettings, IJsonService jsonService)
    {
        _configSettings = configSettings;
        _jsonService = jsonService;
        var dir = string.IsNullOrWhiteSpace(Assembly.GetExecutingAssembly().Location)
            ? AppProfile.SettingsDir
            : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
              AppProfile.SettingsDir;
        _configFilePath = Path.Combine(dir, "config.json");
        Reload();
    }

    /// <summary>
    /// 读取
    /// </summary>
    public void Reload()
    {
        try
        {
            if (!File.Exists(_configFilePath))
                return;
            var jsonStr = File.ReadAllText(_configFilePath);
            var config = _jsonService.DeserializeObject<ConfigSettings>(jsonStr);
            if (config is null)
                return;
            config.MapTo(_configSettings);
        }
        catch (Exception)
        {
            // ignore
        }
    }

    public string GetUrl()
    {
        var url = _configSettings.Host;
        if (!string.IsNullOrEmpty(_configSettings.Port))
        {
            url += ":" + _configSettings.Port;
        }

        url = url.TrimEnd('/');
        return url;
    }

    public string GetApiString(string key)
    {
        var url = GetUrl();
        key = key.TrimStart('/');
        return url + "/" + key;
    }
}