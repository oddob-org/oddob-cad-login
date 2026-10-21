using System.Diagnostics;
using System.Windows.Media;
using MessageBoxIcon = Panuon.WPF.UI.MessageBoxIcon;

namespace Oddob.Service.ViewModel.Account;

[TransientDI]
internal sealed partial class LoginViewModel : ObservableObject
{
    private readonly IAccountService _accountService;
    private readonly AccountSettings _accountSettings;

    public LoginViewModel(IAccountService accountService, AccountSettings accountSettings,
        VerificationCodeSettings verificationCodeSettings)
    {
        _accountService = accountService;
        _accountSettings = accountSettings;
        VerificationCodeSettings = verificationCodeSettings;
        AccountNumber = _accountSettings.Mail;
        RememberPassword = _accountSettings.RememberPassword;
        if (RememberPassword)
        {
            Password = _accountSettings.Password;
        }

        WeakReferenceMessenger.Default.Register<VerificationMessage>(this,
            HandleVerificationMessage);
    }

    private void HandleVerificationMessage(object sender, VerificationMessage message)
    {
        OnPropertyChanged(nameof(VerificationCodeSettings));
        GetVerificationCodeCommand.NotifyCanExecuteChanged();
    }

    /// <summary>
    /// 账号
    /// </summary>
    [ObservableProperty]
    public partial string AccountNumber { get; set; } = "";

    /// <summary>
    /// 密码
    /// </summary>
    [ObservableProperty]
    public partial string Password { get; set; } = "";

    /// <summary>
    /// 需要验证码
    /// </summary>
    [ObservableProperty]
    public partial bool NeedVerification { get; set; }

    /// <summary>
    /// 图片验证码
    /// </summary>
    [ObservableProperty]
    public partial string PngVerifyCode { get; set; } = "";

    /// <summary>
    /// 验证码
    /// </summary>
    [ObservableProperty]
    public partial string VerificationCode { get; set; } = "";

    /// <summary>
    /// 记住密码
    /// </summary>
    [ObservableProperty]
    public partial bool RememberPassword { get; set; }

    /// <summary>
    /// 提示Label
    /// </summary>
    [ObservableProperty]
    public partial string NoticeLabelStr { get; set; } = "";

    /// <summary>
    /// 验证码设置
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(GetVerificationCodeCommand))]
    public partial VerificationCodeSettings VerificationCodeSettings { get; set; }

    public bool CanGetVerificationCode => VerificationCodeSettings.CanGetVerificationCode;

    /// <summary>
    /// 获取验证码
    /// </summary>
    [RelayCommand(CanExecute = (nameof(CanGetVerificationCode)))]
    private async Task GetVerificationCode()
    {
        if (!IsAccountNumberValid)
            NoticeLabelStr = "请输入正确的手机号码";
        var message = NoticeLabelStr = await _accountService.GetPhoneVerificationCode(AccountNumber,
            PngVerifyCode, PngVerificationCodeResultDto);
        ToastMessage.Send(message,
            message.Contains("成功") ? MessageBoxIcon.Success : MessageBoxIcon.Info);
    }

    public bool IsAccountNumberValid =>
        AccountNumber.Length == 11 && AccountNumber.All(char.IsDigit);

    [ObservableProperty]
    public partial bool IsLoginExecuting { get; set; }

    [ObservableProperty]
    public partial ImageSource? VerifyCodeImage { get; set; }

    public PngVerificationCodeResultDto PngVerificationCodeResultDto { get; set; } = new();

    [RelayCommand]
    public async Task RefreshPngVerificationCode()
    {
        VerifyCodeImage = null;
        PngVerificationCodeResultDto = new();
        var responseBody = await _accountService.GetPngVerificationCode();
        if (responseBody is { Success: true, Data: { } data })
        {
            var bitmap = data.VerifyCode.ConvertBase64ToBitmapImage();
            VerifyCodeImage = bitmap;
            if (VerifyCodeImage is not null)
            {
                PngVerificationCodeResultDto = data;
            }
        }
    }

    /// <summary>
    /// 异步执行用户登录操作。
    /// </summary>
    [RelayCommand]
    private async Task Login()
    {
        // 登录操作开始，设置登录按钮状态为执行中，防止重复点击。
        IsLoginExecuting = true;
        // 使用using语句确保在退出作用域时，无论成功还是异常，登录按钮状态都能恢复为非执行中。
        using var _ = DisposeAction.Action(() => IsLoginExecuting = false);
        // 清空通知标签，准备新的通知信息。
        NoticeLabelStr = "";
        // 更新账户设置信息。
        _accountSettings.Mail = AccountNumber;
        _accountSettings.RememberPassword = RememberPassword;
        // 如果选择了记住密码，则同时保存密码到账户设置。
        if (RememberPassword)
            _accountSettings.Password = Password;
        // 保存账户设置到本地存储。
        _accountService.SaveAccountSettings();
        // 验证邮箱是否为空，如果为空则提示用户并返回。
        if (string.IsNullOrWhiteSpace(AccountNumber))
        {
            ToastMessage.Send("账号不能为空", MessageBoxIcon.Info);
            return;
        }

        // if (!IsAccountNumberValid)
        // {
        //     ToastMessage.Send("请输入正确的手机号码", MessageBoxIcon.Info);
        //     return;
        // }

        // 异步发起登录请求，并获取响应结果。
        var responseBody = await _accountService.Login(AccountNumber, Password, VerificationCode);
        // 更新通知标签为服务端返回的消息。
        NoticeLabelStr = responseBody.Message;
        // 如果登录不成功或返回的数据格式不符合预期，则不执行任何操作直接返回。
        if (!responseBody.Success || responseBody.Data is not { } loginDto)
        {
            ToastMessage.Send(responseBody.Message, MessageBoxIcon.Error);
            return;
        }

        NoticeLabelStr = loginDto.Message;
        // 获取登录结果类型。
        var loginResultType = loginDto.AccountResultType;
        // 如果登录结果类型为成功，则通知用户登录成功，并在短暂延迟后发送用户信息更新的消息。
        if (loginResultType == AccountResultType.SUCCESS)
        {
            // NoticeLabelStr = "登录成功";
            ToastMessage.Send("登录成功", MessageBoxIcon.Success);
            await Task.Delay(1500);
            WeakReferenceMessenger.Default.Send(new AccountMessage(AccountPageType.USER_INFO));
            return;
        }
        // 如果登录结果类型为需要验证码或验证码错误，则设置需要验证码的状态为真。

        if (loginResultType is AccountResultType.NEED_VERIFICATION_CODE)
        {
            NeedVerification = true;
        }

        ToastMessage.Send(loginDto.Message, MessageBoxIcon.Info);
    }

    /// <summary>
    /// 切换到注册页面
    /// </summary>
    [RelayCommand]
    private void Register()
    {
        try
        {
            Process.Start("https://oddob.com/#/login");
        }
        catch (Exception e)
        {
            ToastMessage.Send(e.Message, MessageBoxIcon.Info);
        }
    }

    /// <summary>
    /// 切换到密码重置页面
    /// </summary>
    [RelayCommand]
    private void ResetPassword()
    {
        try
        {
            Process.Start("https://oddob.com/#/reset_password");
        }
        catch (Exception e)
        {
            ToastMessage.Send(e.Message, MessageBoxIcon.Info);
        }
    }
}