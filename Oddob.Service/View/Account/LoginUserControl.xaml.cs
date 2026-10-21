using Oddob.Service.ViewModel;
using Oddob.Service.ViewModel.Account;

namespace Oddob.Service.View.Account;

[NavigateViewModel(typeof(LoginViewModel))]
[TransientDI]
sealed partial class LoginUserControl
{
    public LoginUserControl()
    {
        InitializeComponent();
        this.DataContext = AppHelper.Get<LoginViewModel>();
    }

    private async void LoginUserControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (this.DataContext is LoginViewModel loginViewModel)
        {
            await loginViewModel.RefreshPngVerificationCode();
        }
    }
}