namespace Oddob.Service.Constant;

internal static class AppProfile
{
    /// <summary>
    /// App名称
    /// </summary>
    public const string AppName = "Oddob";

    /// <summary>
    /// App目录
    /// </summary>
    public static string AppDir =>
        GetFolder(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            AppName);

    /// <summary>
    /// 设置目录
    /// </summary>
    public static string SettingsDir => GetFolder(AppDir, "Settings");

    /// <summary>
    /// 获取文件夹
    /// </summary>
    /// <param name="pathList">路径列表</param>
    /// <returns>文件夹路径</returns>
    private static string GetFolder(params IEnumerable<string> pathList)
    {
        var dir = Path.Combine(pathList.ToArray());
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        return dir;
    }
}