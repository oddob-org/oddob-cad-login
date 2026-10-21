namespace Oddob.Service.Service.Implement.Account;

[SingletonDI(typeof(IAccountService))]
internal sealed class AccountService : IAccountService
{
    private readonly IComputerInfoService _computerInfoService;
    private readonly IUserSettingService _userSettingService;
    private readonly ICacheService _cacheService;
    private readonly IConfigService _configService;
    private readonly AccountSettings _accountSettings;
    private readonly VerificationCodeSettings _verificationCodeSettings;

    public AccountService(IComputerInfoService computerInfoService, AccountSettings accountSettings,
        IUserSettingService userSettingService, VerificationCodeSettings verificationCodeSettings,
        ICacheService cacheService, IConfigService configService)
    {
        _computerInfoService = computerInfoService;
        _accountSettings = accountSettings;
        _userSettingService = userSettingService;
        _verificationCodeSettings = verificationCodeSettings;
        _cacheService = cacheService;
        _configService = configService;
        ReloadAccountSettings();
    }

    public bool IsLogin
    {
        get
        {
            if (GetIsLogin())
                return true;
            QuiteLogin();
            return GetIsLogin();
        }
    }

    private bool GetIsLogin()
    {
        return _cacheService.IsValidTokenAndUserInfo();
    }

    public async Task<ResponseBody<PngVerificationCodeResultDto>> GetPngVerificationCode()
    {
        var responseBody =
            await Request.GetAsync<PngVerificationCodeResultDto>(
                _configService.GetApiString(ApiConstant.GetPngVerificationCode));
        return responseBody;
    }

    public async Task<string> GetPhoneVerificationCode(string accountNumber, string pngCode,
        PngVerificationCodeResultDto dto)
    {
        // if (!_verificationCodeSettings.CanGetVerificationCode)
        //     return "获取验证码过于频繁，请稍后再试！";
        Dictionary<string, string> dict = new()
        {
            { "accountNumber", accountNumber },
            { "vercode", pngCode },
            { "time", dto.VerifyCodeTime },
            { "hash", dto.VerifyCodeHash }
        };
        var api = _configService.GetApiString(ApiConstant.GetPhoneVerificationCode);
        var responseBody = await Request.PostAsync<PhoneVerifyCodeResultDto>(api, dict);
        if (responseBody is { Success: true, Data: { Success: true } })
        {
            _verificationCodeSettings.StartCountDown();
        }

        return responseBody.Data?.Message ?? responseBody.Message;
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="accountNumber">账号</param>
    /// <param name="password">密码</param>
    /// <param name="verificationCode">验证码</param>
    /// <returns></returns>
    public async Task<ResponseBody<LoginResultDto>> Login(string accountNumber, string password,
        string verificationCode)
    {
        var macId = _computerInfoService.GetMacId();
        if (!string.IsNullOrEmpty(password))
        {
            password = password.GetMd5Hash();
        }

        Dictionary<string, string> dict = new()
        {
            { "macId", macId },
            { "accountNumber", accountNumber },
            { "password", password },
            { "verificationCode", verificationCode }
        };
        var responseBody =
            await Request.PostAsync<LoginResultDto>(_configService.GetApiString(ApiConstant.Login),
                dict);
        if (responseBody is
            {
                Success: true, Data: { AccountResultType: AccountResultType.SUCCESS } loginDto
            })
        {
            _cacheService.SetToken(loginDto);
            _cacheService.SetUserInfo(loginDto.UserInfo);
            // _autoLogin = true;
        }

        return responseBody;
    }

    /*/// <summary>
    /// 自动登录标记
    /// </summary>
    private bool _autoLogin { get; set; } = true;*/

    /// <summary>
    /// 静默登录
    /// </summary>
    /// <returns>成功返回<c>true</c></returns>
    public async Task<bool> QuiteLoginAsync()
    {
        // if (!_autoLogin)
        //     return false;
        var token = _cacheService.GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return false;
        var macId = _computerInfoService.GetMacId();
        Dictionary<string, string> dict = new() { { "macId", macId } };
        var responseBody = await Request.PostAsync<RefreshTokenResultDto>(
            _configService.GetApiString(ApiConstant.RefreshToken), dict);
        if (responseBody is
            {
                Success: true,
                Data: { LoginResultType: AccountResultType.SUCCESS } loginDto,
                StatusCode: 200
            })
        {
            _cacheService.SetToken(loginDto);
            return true;
        }

        // _autoLogin = false;
        _cacheService.LogOut();
        return false;
    }

    /// <summary>
    /// 静默登录
    /// </summary>
    /// <returns>成功返回<c>true</c></returns>
    public bool QuiteLogin()
    {
        // if (!_autoLogin)
        //     return false;
        var token = _cacheService.GetToken();
        if (string.IsNullOrWhiteSpace(token))
            return false;
        var macId = _computerInfoService.GetMacId();
        ResponseBody<RefreshTokenResultDto> responseBody = new();
        try
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(macId, Encoding.UTF8), "macId");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
            // 发送Post请求
            var api = _configService.GetApiString(ApiConstant.RefreshToken);
            var response = client.PostAsync(api, content).GetAwaiter().GetResult();
            responseBody.StatusCode = (int)response.StatusCode;
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var deserializeObject = JsonConvert.DeserializeObject<RefreshTokenResultDto>(result);
            if (deserializeObject != null)
            {
                responseBody.Data = deserializeObject;
                responseBody.Success = true;
            }
        }
        catch (Exception e)
        {
            responseBody.Message = e.Message;
            responseBody.Success = false;
        }

        // var message = responseBody.Data?.Message ?? responseBody.Message;
        if (responseBody is
            {
                Success: true,
                Data: { LoginResultType: AccountResultType.SUCCESS } loginDto,
                StatusCode: 200
            })
        {
            _cacheService.SetToken(loginDto);
            return true;
        }

        // _autoLogin = false;
        _cacheService.SetUserInfo(null);
        return false;
    }

    public void Logout()
    {
        _cacheService.LogOut();
        // _autoLogin = false;
    }

    public void SaveAccountSettings()
    {
        var accountClone = _accountSettings.MapTo<AccountSettings>();
        if (!accountClone.RememberPassword)
        {
            accountClone.Password = "";
        }

        _userSettingService.Save(accountClone);
    }

    public void ReloadAccountSettings()
    {
        _userSettingService.Reload(_accountSettings);
        if (!_accountSettings.RememberPassword)
            _accountSettings.Password = "";
    }
}