using System.Diagnostics;
using System.Windows.Threading;
using Oddob.Service.ViewModel.Account;
using Panuon.WPF.UI;
using Panuon.WPF.UI.Configurations;
using Application = System.Windows.Application;
using Color = System.Windows.Media.Color;
using Label = System.Windows.Controls.Label;
using Brushes = System.Windows.Media.Brushes;

namespace Oddob.Service.View.Account;

[TransientDI]
public partial class AccountWpf
{
    public AccountWpf()
    {
        this.DataContext = AppHelper.Get<AccountViewModel>();
        InitializeComponent();
        WeakReferenceMessenger.Default.Register<AccountCloseMessage>(this, Handler);
        WeakReferenceMessenger.Default.Register<ToastMessage>(this, Handler);
    }

    private void Handler(object recipient, ToastMessage message)
    {
        if (Application.Current is null)
            return;
        var toastSetting = new ToastSetting();
        // 获取基样式
        if (Application.Current.TryFindResource(Panuon.WPF.UI.Toast.LabelStyleKey) is Style
            baseStyle)
        {
            // 创建样式
            var labelStyle = new Style(typeof(Label), baseStyle);

            // 设置标准属性
            labelStyle.Setters.Add(new Setter(BackgroundProperty, Brushes.White));
            labelStyle.Setters.Add(new Setter(ForegroundProperty, Brushes.Black));
            labelStyle.Setters.Add(new Setter(LabelHelper.ShadowColorProperty,
                Color.FromRgb(0x00, 0x99, 0xFF)));
            toastSetting.LabelStyle = labelStyle;
        }

        // 应用到 Toast
        Panuon.WPF.UI.Toast.Show(this, message.Message, message.Icon, message.ToastPosition, -100,
            1000, toastSetting);
    }

    private void Handler(object recipient, AccountCloseMessage message)
    {
        this.Close();
    }

    /*private async void AccountWpf_OnLoaded(object sender, RoutedEventArgs e)
    {
        // await WebView2.EnsureCoreWebView2Async();
        // WebView2.CoreWebView2.Navigate("https://oddob.com/#/landpage");
        // WebView2.CoreWebView2.NavigationStarting -= CoreWebView2OnNavigationStarting;
        // WebView2.CoreWebView2.NavigationStarting += CoreWebView2OnNavigationStarting;
    }*/
    /*private void WebView2_OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
    {
        if(e.Url!="https://oddob.com/#/landpage")
            e.Frame.LoadUrl("https://oddob.com/#/landpage");
    }*/
}

/*public class CustomRequestHandler : IRequestHandler
{
    public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
        IRequest request, bool userGesture, bool isRedirect)
    {
        var url = request.Url;
        if (url == "https://oddob.com/#/landpage")
            return false;

        // 判断是否需要打开外部浏览器，比如某个域名或者所有http/https链接
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开浏览器失败: " + ex.Message);
            }

            // 返回true，阻止CefSharp继续加载该url
            return true;
        }

        // 返回false，允许CefSharp继续加载
        return false;
    }

    public void OnDocumentAvailableInMainFrame(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public bool OnOpenUrlFromTab(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
        string targetUrl, WindowOpenDisposition targetDisposition, bool userGesture)
    {
        MessageBox.Show("123");
        return false;
    }

    public IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser,
        IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload,
        string requestInitiator, ref bool disableDefaultHandling)
    {
        return null;
    }

    public bool GetAuthCredentials(IWebBrowser chromiumWebBrowser, IBrowser browser,
        string originUrl, bool isProxy, string host, int port, string realm, string scheme,
        IAuthCallback callback)
    {
        return false;
    }

    public bool OnCertificateError(IWebBrowser chromiumWebBrowser, IBrowser browser,
        CefErrorCode errorCode, string requestUrl, ISslInfo sslInfo, IRequestCallback callback)
    {
        return false;
    }

    public void OnRenderViewReady(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public void OnRenderProcessTerminated(IWebBrowser chromiumWebBrowser, IBrowser browser,
        CefTerminationStatus status, int errorCode, string errorMessage)
    {
        throw new NotImplementedException();
    }

    public bool OnSelectClientCertificate(IWebBrowser chromiumWebBrowser, IBrowser browser,
        bool isProxy, string host, int port, X509Certificate2Collection certificates,
        ISelectClientCertificateCallback callback) =>
        false;
}

public class CustomLifeSpanHandler : ILifeSpanHandler
{
    public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame,
        string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition,
        bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo,
        IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
    {
        newBrowser = null;

        // 使用系统默认浏览器打开链接
        try
        {
            Process.Start(new ProcessStartInfo(targetUrl) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show("打开浏览器失败: " + ex.Message);
        }

        // 返回true，阻止CefSharp弹出新窗口
        return true;
    }

    public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }

    public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser) => false;

    public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
    {
    }
}*/