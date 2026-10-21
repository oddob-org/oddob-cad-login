namespace Oddob.Service.Service.Interface;

/// <summary>
/// Json序列化服务
/// </summary>
internal interface IJsonService
{
    /// <summary>
    /// 序列化对象
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="jss">json设置</param>
    /// <typeparam name="T">类型</typeparam>
    /// <returns>字符串</returns>
    string SerializeObject<T>(T obj, JsonSerializerSettings? jss = null);

    /// <summary>
    /// 从字符串反序列化
    /// </summary>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="jss">json设置</param>
    /// <typeparam name="T">类型</typeparam>
    /// <returns>对象，失败返回null</returns>
    T? DeserializeObject<T>(string jsonStr, JsonSerializerSettings? jss = null) where T : class;

    /// <summary>
    /// 从字符串反序列化
    /// </summary>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="type">类型</param>
    /// <param name="jss">json设置</param>
    /// <returns>对象，失败返回null</returns>
    object? DeserializeObject(string jsonStr, Type type, JsonSerializerSettings? jss = null);

    /// <summary>
    /// 序列化到文件
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="fileName">文件名</param>
    /// <param name="jss">json设置</param>
    /// <typeparam name="T">类型</typeparam>
    /// <returns>成功返回true</returns>
    bool SerializeToFile<T>(T obj, string fileName, JsonSerializerSettings? jss = null);

    /// <summary>
    /// 从文件反序列化
    /// </summary>
    /// <param name="fileName">文件路径</param>
    /// <param name="jss">json设置</param>
    /// <typeparam name="T">类型</typeparam>
    /// <returns>对象,失败返回null</returns>
    T? DeserializeFromFile<T>(string fileName, JsonSerializerSettings? jss = null) where T : class;
}