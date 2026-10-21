namespace Oddob.Service.Model.Settings;

[SingletonDI]
public class CacheInfo : IUserSetting
{
    public string Token { get; set; } = "";
    public DateTime ExpireTime { get; set; } = DateTime.MinValue;
    public UserInfo? UserInfo { get; set; }
}