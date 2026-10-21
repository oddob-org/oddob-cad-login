using System.Runtime.CompilerServices;

namespace Oddob.Acad;

public static class LispFunc
{
    /*[LispFunction(nameof(Oddob_Check))]
    public static bool Oddob_Check(ResultBuffer? rb)
    {
        var loginIfNot = false;
        var moduleName = "";
        if (rb is not null)
        {
            var typedValues = rb.AsArray();
            if (typedValues.Length > 0)
            {
                var tv0 = typedValues[0];
                // 判断string
                if (tv0.TypeCode == 5005)
                {
                    moduleName = tv0.Value.ToString();
                    if (typedValues.Length > 1)
                    {
                        var tv1 = typedValues[1];
                        loginIfNot = tv1.TypeCode == 5021;
                    }
                }
                else
                {
                    if (tv0.TypeCode == 5021)
                    {
                        loginIfNot = true;
                    }
                }
            }
        }

        var userInfoService = AppHelper.Get<IUserInfoService>();
        var isLogin = userInfoService.IsLogin;
        if (!isLogin)
        {
            if (loginIfNot)
            {
                var dialogResult = MessageBox.Show(@"请先登录！" + Environment.NewLine + "是否现在登录？",
                    @"验证失败", MessageBoxButton.YesNo);
                if (dialogResult != MessageBoxResult.Yes)
                    return false;
                OddobLogin();
                isLogin = userInfoService.IsLogin;
            }
        }

        if (!isLogin )
            return false;
        if(string.IsNullOrEmpty(moduleName))
            return true;
        var has = userInfoService.CheckProduct(moduleName);
        return has;
    }*/

    [CommandMethod(nameof(OddobLogin))]
    public static void OddobLogin()
    {
        var wpf = new AccountWpf();
#if ZWCAD
        wpf.ShowDialog();
#else
        Acap.ShowModalWindow(Acap.MainWindow.Handle, wpf, false);
#endif
    }

    [LispFunction(nameof(Oddob_Login))]
    public static bool Oddob_Login(ResultBuffer? rb)
    {
        var wpf = new AccountWpf();
#if ZWCAD
        wpf.ShowDialog();
#else
        Acap.ShowModalWindow(Acap.MainWindow.Handle, wpf, false);
#endif
        return false;
    }
}