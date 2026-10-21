namespace Oddob.Service.Service.Implement.Account;

[SingletonDI(typeof(IUserInfoService))]
internal sealed class UserInfoService : IUserInfoService
{
    private readonly ILoginRequestService _loginRequestService;
    private readonly Timer _timer;
    private readonly ICacheService _cacheService;
    private readonly IConfigService _configService;
    private readonly IAccountService _accountService;
    private readonly ILispService _lispService;

    /// <summary>
    /// 间隔10分钟
    /// </summary>
    private const int kInterval = 10 * 60 * 1000;

    public UserInfoService(ILoginRequestService loginRequestService, ICacheService cacheService,
        IConfigService configService, IAccountService accountService, ILispService lispService)
    {
        _loginRequestService = loginRequestService;
        _cacheService = cacheService;
        _configService = configService;
        _accountService = accountService;
        _lispService = lispService;
        _timer = new Timer { Interval = kInterval, };
        _timer.Tick += TimerOnTick;
        _timer.Start();
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        _ = RefreshAsync();
    }

    /// <summary>
    /// 刷新用户信息
    /// </summary>
    public void Refresh()
    {
        var api = _configService.GetApiString(ApiConstant.GetUserInfo);
        var responseBody = _loginRequestService.Get<UserInfo>(api);
        IntroduceUserInfo(responseBody);
    }

    public async Task RefreshAsync()
    {
        var api = _configService.GetApiString(ApiConstant.GetUserInfo);
        var responseBody = await _loginRequestService.GetAsync<UserInfo>(api);
        IntroduceUserInfo(responseBody);
    }

    private void IntroduceUserInfo(ResponseBody<UserInfo> responseBody)
    {
        var success = false;
        var type = 0;
        if (responseBody is { Success: true, Data: { } userInfo } &&
            !string.IsNullOrWhiteSpace(userInfo.Tel))
        {
            type = userInfo.Type;
            /*try
            {
                var dict =
                    JsonConvert.DeserializeObject<Dictionary<string, Product>>(
                        JsonConvert.SerializeObject(userInfo.ProductsJson));
                if (dict != null)
                {
                    userInfo.Products = dict.Select(e => e.Value).ToList();
                }
                else
                {
                    userInfo.Products = [];
                }
            }
            catch (Exception)
            {
                // 忽略
            }*/
            userInfo.Products = userInfo.ProductsJson.ToList();
            _cacheService.SetUserInfo(userInfo);
            _timer.Interval = kInterval;
            if (userInfo.CurrentProduct is {} product)
            {
                success = !product.IsOver;
            }
        }
        else
        {
            _cacheService.SetUserInfo(null);
            // 请求失败,15秒后再次刷新
            _timer.Interval = 15 * 1000;
        }

        HashSet<string> lispSymbols = [];
        if (success)
        {
            if (type !=0)
            {
                lispSymbols.Add("@oddent-inte");
                lispSymbols.Add("@oddentoption-inte");
            }
            else
            {
                lispSymbols.Add("@oddpro-inte");
                lispSymbols.Add("@oddoption-inte");
            }
        }
        else
        {
            lispSymbols.Add("@oddpro-inte");
            lispSymbols.Add("@oddoption-inte");
            lispSymbols.Add("@oddent-inte");
            lispSymbols.Add("@oddentoption-inte");
        }

        foreach (var lispSymbol in lispSymbols)
        {
            _lispService.Setq(lispSymbol, success ? "T" : "nil");
        }
    }

    public bool IsLogin
    {
        get
        {
            if (!_cacheService.IsValidTokenAndUserInfo())
            {
                _accountService.QuiteLogin();
                Refresh();
            }

            return _cacheService.IsValidTokenAndUserInfo();
        }
    }

    public async Task<bool> GetIsLogin()
    {
        if (!_cacheService.IsValidTokenAndUserInfo())
        {
            await _accountService.QuiteLoginAsync();
            await RefreshAsync();
        }

        return _cacheService.IsValidTokenAndUserInfo();
    }

    public bool CheckProduct(string productName)
    {
        var userInfo = _cacheService.GetUserInfo();
        if (userInfo is null)
            return false;
        return userInfo.Products.Any(p =>
            p.Name.Equals(productName, StringComparison.OrdinalIgnoreCase));
    }

    public List<Product> GetProducts()
    {
        return _cacheService.GetUserInfo()?.Products.ToList() ?? [];
    }
}