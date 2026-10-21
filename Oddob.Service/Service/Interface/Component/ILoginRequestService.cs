namespace Oddob.Service.Service.Interface.Component;

internal interface ILoginRequestService
{
    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="apiString">Api字符串</param>
    /// <typeparam name="T">响应体数据类型</typeparam>
    /// <returns>响应体</returns>
    Task<ResponseBody<T>> GetAsync<T>(string apiString) where T : notnull;

    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="apiString">Api字符串</param>
    /// <typeparam name="T">响应体数据类型</typeparam>
    /// <returns>响应体</returns>
    ResponseBody<T> Get<T>(string apiString) where T : notnull;

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    Task<ResponseBody<TX>> PostAsync<TQ, TX>(string apiString, TQ requestBody) where TX : notnull;

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    ResponseBody<TX> Post<TQ, TX>(string apiString, TQ requestBody) where TX : notnull;
}