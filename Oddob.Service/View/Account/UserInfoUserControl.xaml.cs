using Oddob.Service.ViewModel;
using Oddob.Service.ViewModel.Account;

namespace Oddob.Service.View.Account;

[NavigateViewModel(typeof(LoginViewModel))]
[TransientDI]
sealed partial class UserInfoUserControl
{
    public UserInfoUserControl()
    {
        InitializeComponent();
        this.DataContext = AppHelper.Get<UserInfoViewModel>();
    }

    // ReSharper disable once AsyncVoidMethod
    private async void UserInfoUserControl_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (DataContext is UserInfoViewModel viewModel)
        {
            await viewModel.Refresh();
        }
    }
}