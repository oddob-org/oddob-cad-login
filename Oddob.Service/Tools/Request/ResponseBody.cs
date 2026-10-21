namespace Oddob.Service.Tools.Request;

/// <summary>
/// 响应体
/// </summary>
internal sealed class ResponseBody<T>
{
    /// <summary>
    /// 成功
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string Message { get; set; } = "";

    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; } =400;
}