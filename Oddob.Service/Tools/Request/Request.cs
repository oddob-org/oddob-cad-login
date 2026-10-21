using System.Net;
using Newtonsoft.Json.Linq;

namespace Oddob.Service.Tools.Request;

internal static class Request
{
    static Request()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <typeparam name="T">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public static async Task<ResponseBody<T>> GetAsync<T>(string apiString)
    {
        ResponseBody<T> responseBody = new();
        using var client = new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            // 发送Post请求
            var response = await client.GetAsync(apiString);
            responseBody.StatusCode = (int)response.StatusCode;

            // 断言
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var deserializeObject = JsonConvert.DeserializeObject<T>(result);
            if (deserializeObject != null)
            {
                responseBody.Data = deserializeObject;
                responseBody.Success = true;
            }
        }
        catch (Exception e)
        {
            responseBody.Message = e.Message;
        }

        return responseBody;
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <typeparam name="T">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public static ResponseBody<T> Get<T>(string apiString)
    {
        ResponseBody<T> responseBody = new();
        using var client = new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            // 发送Post请求
            var response = client.GetAsync(apiString).Result;
            // 断言
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            responseBody.StatusCode = (int)response.StatusCode;
            var deserializeObject = JsonConvert.DeserializeObject<ResponseBody<T>>(result);
            if (deserializeObject != null)
            {
                responseBody = deserializeObject;
            }

            responseBody.StatusCode = (int)response.StatusCode;
        }
        catch (Exception e)
        {
            responseBody.Message = e.Message;
        }

        return responseBody;
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public static async Task<ResponseBody<TX>> PostAsync<TQ, TX>(string apiString, TQ requestBody)
        where TQ : notnull
    {
        ResponseBody<TX> responseBody = new();
        using var client = new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            var jObject = JObject.FromObject(requestBody);
            using var content = new MultipartFormDataContent();
            foreach (var property in jObject.Properties())
            {
                content.Add(new StringContent(property.Value.ToString()), property.Name);
            }

            // 发送Post请求
            var response = await client.PostAsync(apiString, content);
            responseBody.StatusCode = (int)response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var deserializeObject = JsonConvert.DeserializeObject<TX>(result);
            {
                if (deserializeObject != null)
                    responseBody.Data = deserializeObject;
            }
            responseBody.StatusCode = (int)response.StatusCode;
        }
        catch (Exception e)
        {
            var message = e.Message;
            if (e is AggregateException ae)
            {
                message = string.Join(Environment.NewLine,
                    ae.InnerExceptions.Select(ie => ie.ToString()));
            }

            responseBody.Message = message;
        }

        return responseBody;
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="multiData">请求体</param>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public static async Task<ResponseBody<TX>> PostAsync<TX>(string apiString,
        Dictionary<string, string> multiData)
    {
        ResponseBody<TX> responseBody = new();
        using var client = new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            using var content = new MultipartFormDataContent();
            foreach (var kvp in multiData)
            {
                content.Add(new StringContent(kvp.Value,Encoding.UTF8), kvp.Key);
            }

            // 发送Post请求
            var response = await client.PostAsync(apiString, content);
            responseBody.StatusCode = (int)response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var deserializeObject = JsonConvert.DeserializeObject<TX>(result);
            {
                if (deserializeObject != null)
                {
                    responseBody.Data = deserializeObject;
                    responseBody.Success = true;
                }
            }
        }
        catch (Exception e)
        {
            responseBody.Message = e.Message;
            responseBody.Success = false;
        }

        return responseBody;
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public static ResponseBody<TX> Post<TQ, TX>(string apiString, TQ requestBody)
    {
        ResponseBody<TX> responseBody = new();
        using var client = new HttpClient();
        try
        {
            client.Timeout = TimeSpan.FromSeconds(5);
            // 字符串
            var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8, "application/json");
            // 发送Post请求
            var response = client.PostAsync(apiString, stringContent).Result;
            responseBody.StatusCode = (int)response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;
            var deserializeObject = JsonConvert.DeserializeObject<ResponseBody<TX>>(result);
            {
                if (deserializeObject != null)
                    responseBody = deserializeObject;
            }
            responseBody.StatusCode = (int)response.StatusCode;
        }
        catch (Exception e)
        {
            var message = e.Message;
            if (e is AggregateException ae)
            {
                message = string.Join(Environment.NewLine,
                    ae.InnerExceptions.Select(ie => ie.ToString()));
            }

            responseBody.Message = message;
        }

        return responseBody;
    }
}