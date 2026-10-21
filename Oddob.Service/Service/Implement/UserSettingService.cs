namespace Oddob.Service.Service.Implement;

/// <summary>
/// 用户设置服务
/// </summary>
/// <remarks>
/// 该类负责用户设置的加载和保存操作。
/// </remarks>
[SingletonDI(typeof(IUserSettingService))]
internal sealed class UserSettingService : IUserSettingService
{
    private readonly IJsonService _jsonService;
    private readonly FileSystemWatcher _fileSystemWatcher;
    private readonly Dictionary<string, Type> _dict = [];

    /// <summary>
    /// 初始化 UserSettingService 类的新实例
    /// </summary>
    /// <param name="jsonService">JSON 服务</param>
    public UserSettingService(IJsonService jsonService)
    {
        _jsonService = jsonService;

        _fileSystemWatcher = new FileSystemWatcher()
        {
            Path = GetDir(), Filter = "*.json", NotifyFilter = NotifyFilters.LastWrite,
        };
        _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
        _fileSystemWatcher.EnableRaisingEvents = true;
    }

    /// <summary>
    /// 标记事件是否在运行
    /// </summary>
    private bool _running = true;

    private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs e)
    {
        if (!_running)
            return;
        var name = Path.GetFileNameWithoutExtension(e.Name);
        if (string.IsNullOrWhiteSpace(name) || !_dict.TryGetValue(name, out var type) ||
            AppHelper.TryGet(type) is not IUserSetting setting)
            return;
        Reload(setting);
    }

    /// <summary>
    /// 获取与给定对象关联的文件名
    /// </summary>
    /// <param name="obj">用户设置对象</param>
    /// <returns>文件名，包括路径</returns>
    private string GetFileName(object obj)
    {
        return Path.Combine(GetDir(), obj.GetType().Name + ".json");
    }

    private static string GetDir()
    {
        var dir = AppProfile.SettingsDir;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        return dir;
    }

    /// <summary>
    /// 从文件中重新加载用户设置
    /// </summary>
    /// <param name="userSetting">用户设置接口</param>
    /// <returns>操作是否成功</returns>
    public bool Reload(IUserSetting userSetting)
    {
        try
        {
            var fileName = GetFileName(userSetting);
            var jsonStr = File.ReadAllText(fileName);
            var deserializeObject = _jsonService.DeserializeObject(jsonStr, userSetting.GetType());
            if (deserializeObject == null)
                return false;
            deserializeObject.MapTo(userSetting);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// 将用户设置保存到文件
    /// </summary>
    /// <param name="userSetting">用户设置接口</param>
    /// <returns>操作是否成功</returns>
    public bool Save(IUserSetting userSetting)
    {
        try
        {
            _running = false;
            var fileName = GetFileName(userSetting);
            var jsonStr = _jsonService.SerializeObject(userSetting);
            File.WriteAllText(fileName, jsonStr);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            _running = true;
        }
    }

    public void ReloadAll()
    {
        var iType = typeof(IUserSetting);
        var settingsTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t =>
                t.IsClass && iType.IsAssignableFrom(t) &&
                t.GetCustomAttribute<SingletonDIAttribute>() is not null)
            .ToList();
        foreach (var settingsType in settingsTypes)
        {
            if (AppHelper.Get(settingsType) is IUserSetting settings)
            {
                Reload(settings);
                _dict[settingsType.Name] = settingsType;
            }
        }
    }
}