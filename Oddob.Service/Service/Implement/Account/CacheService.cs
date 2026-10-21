using Oddob.Service.Model.Settings;

namespace Oddob.Service.Service.Implement.Account;

[SingletonDI(typeof(ICacheService))]
internal sealed class CacheService : ICacheService
{
    private readonly CacheInfo _cacheInfo;
    private readonly IUserSettingService _userSettingService;

    public CacheService(CacheInfo cacheInfo, IUserSettingService userSettingService)
    {
        _cacheInfo = cacheInfo;
        _userSettingService = userSettingService;
    }

    public string GetToken()
    {
        return _cacheInfo.Token;
    }

    public void SetToken(LoginResultDto loginResultDto)
    {
        _cacheInfo.Token = loginResultDto.Token;
        _cacheInfo.ExpireTime = DateTimeOffset
            .FromUnixTimeSeconds(loginResultDto.UserInfo.TokenExpireTime)
            .DateTime;
        _userSettingService.Save(_cacheInfo);
    }

    public void SetToken(RefreshTokenResultDto refreshTokenResult)
    {
        _cacheInfo.Token = refreshTokenResult.Token;
        _cacheInfo.ExpireTime = DateTimeOffset
            .FromUnixTimeSeconds(refreshTokenResult.Expire)
            .DateTime;
    }

    public void SetUserInfo(UserInfo? userInfo)
    {
        _cacheInfo.UserInfo = userInfo;
        _userSettingService.Save(_cacheInfo);
    }

    public UserInfo? GetUserInfo()
    {
        return _cacheInfo.UserInfo;
    }

    public void LogOut()
    {
        _cacheInfo.Token = "";
        _cacheInfo.ExpireTime = DateTime.MinValue;
        _cacheInfo.UserInfo = null;
        _userSettingService.Save(_cacheInfo);
    }

    public bool IsValidTokenAndUserInfo()
    {
        return !string.IsNullOrWhiteSpace(_cacheInfo.Token) &&
               _cacheInfo.ExpireTime > DateTime.Now && _cacheInfo.UserInfo is not null;
    }
}