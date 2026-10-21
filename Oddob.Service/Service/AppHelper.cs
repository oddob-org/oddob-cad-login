using Microsoft.Extensions.DependencyInjection;
using Oddob.Service;

// ReSharper disable once CheckNamespace
namespace DYH.CAD.Extensions;

/// <summary>
/// 提供应用程序服务初始化和依赖注入的帮助方法
/// </summary>
internal static class AppHelper
{
    /// <summary>
    /// 初始化服务提供程序通过设置依赖注入并添加程序集
    /// </summary>
    internal static void Init()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.SetUpDependencyInjection()
            .AddAssembly(Initialize.Assemblies)
            .BeginInject();
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    /// <summary>
    /// 用于依赖解析的服务提供程序实例
    /// </summary>
    private static IServiceProvider _serviceProvider = new NotImplementedProvider();

    /// <summary>
    /// 获取指定类型的实例
    /// </summary>
    /// <typeparam name="T">要获取的实例类型</typeparam>
    /// <returns>服务实例</returns>
    public static T Get<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

    /// <summary>
    /// 获取指定类型的实例
    /// </summary>
    /// <param name="t">要获取的实例类型</param>
    /// <returns>服务实例</returns>
    public static object Get(Type t) => _serviceProvider.GetRequiredService(t);

    /// <summary>
    /// 尝试获取指定类型的实例
    /// </summary>
    /// <typeparam name="T">要获取的实例类型</typeparam>
    /// <returns>如果找到则返回服务实例 否则返回null</returns>
    public static T? TryGet<T>() where T : notnull => _serviceProvider.GetService<T>();

    /// <summary>
    /// 尝试获取指定类型的实例
    /// </summary>
    /// <param name="t">要获取的实例类型</param>
    /// <returns>如果找到则返回服务实例 否则返回null</returns>
    public static object? TryGet(Type t) => _serviceProvider.GetService(t);
}

internal class NotImplementedProvider : IServiceProvider
{
    public object GetService(Type serviceType)
    {
        throw new NotImplementedException();
    }
}