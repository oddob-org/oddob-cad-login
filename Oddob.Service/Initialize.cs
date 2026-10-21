using System.Net;
using Application = System.Windows.Application;

namespace Oddob.Service;

/// <summary>
/// 提供初始化功能的静态类，用于配置和设置程序集
/// </summary>
public static class Initialize
{
    internal static Assembly[] Assemblies { get; private set; } = [];

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public static Application? App { get; private set; }

    /// <summary>
    /// 初始化配置并设置程序集列表
    /// </summary>
    public static void Setup(params IEnumerable<Assembly> assemblies)
    {
        if (Application.Current == null)
        {
            App = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
        }

        ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
        Debug();
        Assemblies = assemblies.Concat([Assembly.GetExecutingAssembly()]).ToHashSet().ToArray();
        AppHelper.Init();
        var userSettingService = AppHelper.Get<IUserSettingService>();
        userSettingService.ReloadAll();
        AppHelper.Get<IAccountService>().QuiteLogin();
        AppHelper.Get<IUserInfoService>().Refresh();
        // CefSharp.Cef.Initialize(new CefSettings());
    }

    private static void Debug()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainOnAssemblyResolve;
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        _ = new DebugBehaviors();
    }

    #region Debug程序集

    public class DebugBehaviors : Microsoft.Xaml.Behaviors.EventTrigger;

    #endregion

    /// <summary>
    /// debug程序集
    /// </summary>
    private static Assembly? CurrentDomainOnAssemblyResolve(object? sender, ResolveEventArgs args)
    {
        if (sender is not AppDomain domain)
            return null;
        var assemblyName = new AssemblyName(args.Name);
        if (assemblyName.Name.EndsWith("resources"))
            return null;
        domain.AssemblyResolve -= CurrentDomainOnAssemblyResolve;
        try
        {
            var ass = Assembly.Load(assemblyName.Name!);
            return ass;
        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            domain.AssemblyResolve -= CurrentDomainOnAssemblyResolve;
            domain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }
    }

    public static void Refresh()
    {
        AppHelper.Get<IUserInfoService>().Refresh();
    }
}