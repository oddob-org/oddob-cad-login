using Oddob.Service.View.Account;

namespace Oddob.Service.ViewModel.Account;

[TransientDI]
internal sealed partial class AccountViewModel : ObservableObject
{
    public AccountViewModel(IUserInfoService userInfoService)
    {
        WeakReferenceMessenger.Default.Register<AccountMessage>(this, HandlerAccountMessage);
        if (userInfoService.IsLogin)
        {
            CurrentPageType = AccountPageType.USER_INFO;
        }
        ResetContentPage();
    }

    [ObservableProperty]
    public partial AccountPageType CurrentPageType { get; set; } = AccountPageType.LOGIN;

    private void HandlerAccountMessage(object sender, AccountMessage message)
    {
        CurrentPageType = message.AccountPageType;
        ResetContentPage();
    }

    private void ResetContentPage()
    {
        CurrentPage = CurrentPageType switch
        {
            AccountPageType.LOGIN => AppHelper.Get<LoginUserControl>(),
            // AccountPageType.REGISTER => AppHelper.Get<RegisterUserControl>(),
            // AccountPageType.RESET_PASSWORD => AppHelper.Get<ResetPasswordUserControl>(),
            AccountPageType.USER_INFO => AppHelper.Get<UserInfoUserControl>(),
            _ => CurrentPage
        };
        Title= CurrentPageType.GetDescription();
        CurrentContent = CurrentPage?.DataContext as ObservableObject;
    }

    [ObservableProperty]
    public partial string Title { get; set; } = "";

    [ObservableProperty]
    public partial ObservableObject? CurrentContent { get; set; }

    [ObservableProperty]
    public partial Page? CurrentPage { get; set; }
}