namespace Oddob.Service.Service.Implement;

/// <summary>
/// json服务
/// </summary>
[SingletonDI(typeof(IJsonService))]
internal sealed class JsonService : IJsonService
{
    /// <summary>
    /// json默认配置
    /// </summary>
    private readonly JsonSerializerSettings _jsonOptions = new()
    {
        Formatting = Formatting.Indented
    };

    public string SerializeObject<T>(T obj, JsonSerializerSettings? jss = null)
    {
        return JsonConvert.SerializeObject(obj, jss ?? _jsonOptions);
    }

    public T? DeserializeObject<T>(string jsonStr, JsonSerializerSettings? jss = null)
        where T : class
    {
        try
        {
            return JsonConvert.DeserializeObject<T>(jsonStr, jss ?? _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public object? DeserializeObject(string jsonStr, Type type, JsonSerializerSettings? jss = null)
    {
        try
        {
            return JsonConvert.DeserializeObject(jsonStr, type, jss ?? _jsonOptions);
        }
        catch
        {
            return null;
        }
    }

    public bool SerializeToFile<T>(T obj, string fileName, JsonSerializerSettings? jss = null)
    {
        try
        {
            var jsonStr = SerializeObject(obj, jss);
            File.WriteAllText(fileName, jsonStr);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public T? DeserializeFromFile<T>(string fileName, JsonSerializerSettings? jss = null)
        where T : class
    {
        if (!File.Exists(fileName))
        {
            return null;
        }

        var jsonStr = File.ReadAllText(fileName);
        return DeserializeObject<T>(jsonStr, jss);
    }
}