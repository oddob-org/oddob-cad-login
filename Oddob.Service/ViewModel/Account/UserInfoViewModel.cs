using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using Panuon.WPF.UI;

namespace Oddob.Service.ViewModel.Account;

[TransientDI]
internal partial class UserInfoViewModel : ObservableObject
{
    private readonly IAccountService _accountService;
    private readonly ICacheService _cacheService;
    private readonly IUserInfoService _userInfoService;

    public UserInfoViewModel(IAccountService accountService, ICacheService cacheService,
        IUserInfoService userInfoService)
    {
        _accountService = accountService;
        _cacheService = cacheService;
        _userInfoService = userInfoService;
    }

    [RelayCommand]
    public async Task Refresh()
    {
        if (!await _userInfoService.GetIsLogin() || _cacheService.GetUserInfo() is not { } userInfo)
        {
            WeakReferenceMessenger.Default.Send(new AccountMessage(AccountPageType.LOGIN));
            return;
        }
        await _userInfoService.RefreshAsync();

        var info = new UserInfoModel();
        userInfo.MapTo(info);
        info.HeadImage = string.IsNullOrWhiteSpace(info.Litpic)
            ? new BitmapImage(new Uri(
                "pack://application:,,,/Oddob.Service;component/Resources/logo.png",
                UriKind.RelativeOrAbsolute))
            : new BitmapImage(new Uri(info.Litpic, UriKind.Absolute));
        UserInfo = info;
        Products = new ObservableCollection<Product>(_userInfoService.GetProducts());
    }

    [ObservableProperty]
    public partial UserInfoModel UserInfo { get; set; } = new();

    [ObservableProperty]
    public partial ObservableCollection<Product> Products { get; set; } = [];

    [RelayCommand]
    private void Buy()
    {
        try
        {
            Process.Start("https://oddob.com");
        }
        catch (Exception e)
        {
            ToastMessage.Send(e.Message, MessageBoxIcon.Info);
        }
    }
    /// <summary>
    /// 执行用户登出操作。
    /// </summary>
    /// 发送登录信息，以通知其他部分用户已登出。
    [RelayCommand]
    private void Logout()
    {
        _accountService.Logout();
        _userInfoService.Refresh();
        WeakReferenceMessenger.Default.Send(new AccountMessage(AccountPageType.LOGIN));
    }

    /// <summary>
    /// 关闭指定的窗口。
    /// </summary>
    /// 通过查找视觉父级方法，确保可以关闭任何嵌套窗口的父窗口。
    [RelayCommand]
    private void Close()
    {
        WeakReferenceMessenger.Default.Send(new AccountCloseMessage());
    }
}