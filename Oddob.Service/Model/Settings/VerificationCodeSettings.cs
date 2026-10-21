namespace Oddob.Service.Model.Settings;


[SingletonDI]
public partial class VerificationCodeSettings : ObservableObject
{
    [ObservableProperty]
    public partial bool CanGetVerificationCode { get; set; } = true;

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnCanGetVerificationCodeChanged(bool value)
    {
        WeakReferenceMessenger.Default.Send(new VerificationMessage());
    }

    private DateTime _lastRequestTime;

    [ObservableProperty]
    public partial string ButtonText { get; set; } = "获取验证码";

    // ReSharper disable once UnusedParameterInPartialMethod
    partial void OnButtonTextChanged(string value)
    {
        WeakReferenceMessenger.Default.Send(new VerificationMessage());
    }

    private Timer? _timer;

    // 倒计时
    public void StartCountDown()
    {
        _lastRequestTime = DateTime.Now;
        _timer = new Timer() { Interval = 200, };
        _timer.Tick += TimerOnTick;
        _timer.Start();
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        var timeSpan = DateTime.Now - _lastRequestTime;
        var second = 60 - timeSpan.Seconds;
        if (timeSpan.Minutes > 0)
        {
            if (sender is Timer timer)
            {
                timer.Dispose();
            }

            _timer = null;
            CanGetVerificationCode = true;
            ButtonText = "获取验证码";
        }
        else
        {
            CanGetVerificationCode = false;
            ButtonText = $"重新获取({second})";
        }
    }
}