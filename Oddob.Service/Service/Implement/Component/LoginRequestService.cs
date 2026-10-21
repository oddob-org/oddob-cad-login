namespace Oddob.Service.Service.Implement.Component;

/// <summary>
/// 需要登录后才能访问的接口请求服务
/// </summary>
[SingletonDI(typeof(ILoginRequestService))]
internal sealed class LoginRequestService : ILoginRequestService
{
    /// <summary>
    /// 用于存储令牌信息的对象，标记为只读以确保在构造函数中初始化后不再修改。
    /// </summary>
    private readonly ICacheService _cacheService;

    /// <summary>
    /// 用于访问账户服务的接口，标记为只读以确保在构造函数中初始化后不再修改。
    /// </summary>
    private readonly IAccountService _accountService;

    /// <summary>
    /// 定义一个常量<see cref="F:kTimeOut"/>，用于指定操作的超时时间，单位为秒。这里设置为10秒。
    /// </summary>
    private const int kTimeOut = 10;

    public LoginRequestService(IAccountService accountService, ICacheService cacheService)
    {
        _accountService = accountService;
        _cacheService = cacheService;
    }

    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="apiString">Api字符串</param>
    /// <typeparam name="T">响应体数据类型</typeparam>
    /// <returns>响应体</returns>
    public async Task<ResponseBody<T>> GetAsync<T>(string apiString) where T : notnull
    {
        return await QuietRequestAsync<T>(async client => await client.GetAsync(apiString));
    }

    /// <summary>
    /// 发送Get请求
    /// </summary>
    /// <param name="apiString">Api字符串</param>
    /// <typeparam name="T">响应体数据类型</typeparam>
    /// <returns>响应体</returns>
    public ResponseBody<T> Get<T>(string apiString) where T : notnull
    {
        return QuietRequest<T>(client => client.GetAsync(apiString).Result);
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public async Task<ResponseBody<TX>> PostAsync<TQ, TX>(string apiString, TQ requestBody)
        where TX : notnull
    {
        return await QuietRequestAsync<TX>(async client =>
        {
            // 字符串
            var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8, "application/json");
            // 发送Post请求
            return await client.PostAsync(apiString, stringContent);
        });
    }

    /// <summary>
    /// 发送Post请求
    /// </summary>
    /// <param name="apiString">api字符串</param>
    /// <param name="requestBody">请求体</param>
    /// <typeparam name="TQ">请求体类型</typeparam>
    /// <typeparam name="TX">响应体类型</typeparam>
    /// <returns>响应体</returns>
    public ResponseBody<TX> Post<TQ, TX>(string apiString, TQ requestBody) where TX : notnull
    {
        return QuietRequest<TX>(client =>
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8, "application/json");
            // 发送Post请求
            return client.PostAsync(apiString, stringContent).Result;
        });
    }

    /// <summary>
    /// 异步执行“静默”请求，用于在令牌过期时自动刷新令牌。
    /// </summary>
    /// <typeparam name="TX">返回体中数据的类型。</typeparam>
    /// <param name="func">一个Func委托，用于发送HTTP请求并返回响应消息。</param>
    /// <returns>返回一个包含响应数据的任务对象。</returns>
    private async Task<ResponseBody<TX>> QuietRequestAsync<TX>(
        Func<HttpClient, Task<HttpResponseMessage>> func) where TX : notnull
    {
        // 发起自定义请求，并获取响应。
        var result = await CustomRequestAsync<TX>(async client => await func.Invoke(client));
        // 如果响应状态码不是禁止或未授权，则直接返回结果。
        if (result.Success)
            return result;

        // 如果响应状态码是禁止或未授权，尝试再次刷新令牌并重新发起请求。
        await _accountService.QuiteLoginAsync();
        return await CustomRequestAsync<TX>(async client => await func.Invoke(client));
    }

    /// <summary>
    /// 执行“静默”请求，用于在令牌过期时自动刷新令牌。
    /// </summary>
    /// <typeparam name="TX">返回体中数据的类型。</typeparam>
    /// <param name="func">一个Func委托，用于发送HTTP请求并返回响应消息。</param>
    /// <returns>返回包含响应数据的对象。</returns>
    private ResponseBody<TX> QuietRequest<TX>(Func<HttpClient, HttpResponseMessage> func)
        where TX : notnull
    {
        // 发起自定义请求，并获取响应。
        var result = CustomRequest<TX>(func.Invoke);
        // 如果响应状态码不是禁止或未授权，则直接返回结果。
        if (result.Success)
            return result;

        // 如果响应状态码是禁止或未授权，尝试再次刷新令牌并重新发起请求。
        _accountService.QuiteLogin();
        return CustomRequest<TX>(func.Invoke);
    }

    /// <summary>
    /// 异步执行自定义HTTP请求，包含错误处理。
    /// </summary>
    /// <typeparam name="TX">返回体中数据的类型。</typeparam>
    /// <param name="func">一个Func委托，用于发送HTTP请求并返回响应消息。</param>
    /// <returns>返回一个包含响应数据的任务对象。</returns>
    private async Task<ResponseBody<TX>> CustomRequestAsync<TX>(
        Func<HttpClient, Task<HttpResponseMessage>> func) where TX : notnull
    {
        // 使用using语句确保HttpClient在使用后被正确释放。
        using var client = new HttpClient();
        // 设置请求超时时间。
        SetTimeOut(client);
        // 设置请求的令牌。
        SetToken(client);
        // 发起请求并获取响应。
        try
        {
            var response = await func.Invoke(client);
            // 解析响应体。
            var body = GetResponseBody<TX>(response);
            return body;
        }
        catch (Exception e)
        {
            return new ResponseBody<TX> { Message = e.Message };
        }
    }

    /// <summary>
    /// 执行自定义HTTP请求，包含错误处理。
    /// </summary>
    /// <typeparam name="TX">返回体中数据的类型。</typeparam>
    /// <param name="func">一个Func委托，用于发送HTTP请求并返回响应消息。</param>
    /// <returns>返回包含响应数据的对象。</returns>
    private ResponseBody<TX> CustomRequest<TX>(Func<HttpClient, HttpResponseMessage> func)
        where TX : notnull
    {
        // 初始化返回体。
        // 使用using语句确保HttpClient在使用后被正确释放。
        using var client = new HttpClient();
        try
        {
            // 设置请求超时时间。
            SetTimeOut(client);
            // 设置请求的令牌。
            SetToken(client);
            // 发起请求并获取响应。
            var response = func.Invoke(client);
            // 解析响应体。
            var body = GetResponseBody<TX>(response);
            return body;
        }
        catch (Exception ex)
        {
            // 在捕获到异常时，将异常消息写入返回体。
            return new ResponseBody<TX> { Message = ex.Message, };
        }
    }

    /// <summary>
    /// 设置HttpClient的Token认证信息
    /// </summary>
    /// <param name="client">待设置Token的HttpClient实例</param>
    private void SetToken(HttpClient client)
    {
        // 设置HttpClient的默认请求头，用于携带认证Token
        var token = _cacheService.GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
    }

    /// <summary>
    /// 设置HttpClient的超时时间。
    /// </summary>
    /// <param name="client">HttpClient实例。</param>
    /// <remarks>
    /// 该方法用于统一配置HttpClient的超时参数，确保所有网络请求在超时设置上的一致性。
    /// </remarks>
    private static void SetTimeOut(HttpClient client)
    {
        // 设置HttpClient的超时时间为kTimeOut秒
        client.Timeout = TimeSpan.FromSeconds(kTimeOut);
    }

    /// <summary>
    /// 获取HTTP响应的消息体
    /// </summary>
    /// <param name="response">HTTP响应消息对象</param>
    /// <typeparam name="T">响应消息体中数据的类型</typeparam>
    private ResponseBody<T> GetResponseBody<T>(HttpResponseMessage response) where T : notnull
    {
        var body = new ResponseBody<T>();
        try
        {
            // 设置响应状态码
            body.StatusCode = (int)response.StatusCode;
            response.EnsureSuccessStatusCode();

            // 读取响应内容为字符串
            var result = response.Content.ReadAsStringAsync()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            // 尝试反序列化响应内容
            var deserializeObject = JsonConvert.DeserializeObject<T>(result);
            if (deserializeObject != null)
            {
                body.Data = deserializeObject;
                body.Success = true;
            }
        }
        catch (Exception ex)
        {
            body.Message = ex.Message;
            body.Success = false;
        }

        return body;
    }
}