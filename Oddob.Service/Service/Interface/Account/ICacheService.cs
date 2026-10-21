namespace Oddob.Service.Service.Interface.Account;

internal interface ICacheService
{
    public string GetToken();
    public void SetToken(LoginResultDto loginResultDto);
    public void SetToken(RefreshTokenResultDto refreshTokenResult);
    public void SetUserInfo(UserInfo? userInfo);
    public UserInfo? GetUserInfo();
    public void LogOut();
    public bool IsValidTokenAndUserInfo();
}