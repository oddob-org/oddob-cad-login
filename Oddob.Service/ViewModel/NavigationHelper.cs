namespace Oddob.Service.ViewModel;

/// <summary>
/// 提供导航相关的辅助功能，主要用于根据 ViewModel 类型查找对应的 View 类型。
/// </summary>
public static class NavigationHelper
{
    /// <summary>
    /// 静态构造函数，初始化时扫描当前程序集中的所有类，
    /// 并将标记了 <see cref="NavigateViewModelAttribute"/> 特性的类与其关联的 ViewModel 类型存储到字典中。
    /// </summary>
    static NavigationHelper()
    {
        // 获取当前程序集中的所有非静态类
        var types = Initialize.Assemblies.SelectMany(e=>e.GetTypes())
            // ReSharper disable once MergeIntoPattern
            .Where(e => e.IsClass && !(e.IsAbstract && e.IsSealed))
            .ToList();

        // 遍历所有类，检查是否标记了 NavigateViewModelAttribute 特性
        foreach (var type in types)
        {
            if (type.GetCustomAttribute<NavigateViewModelAttribute>() is not { } attribute)
                continue;

            // 将 ViewModel 类型与对应的 View 类型存入字典
            _dict.Add(attribute.ViewModelType, type);
        }
    }

    // 存储 ViewModel 类型与 View 类型的映射关系
    private static readonly Dictionary<Type, Type> _dict = [];

    /// <summary>
    /// 根据指定的 ViewModel 类型查找对应的 View 类型。
    /// </summary>
    /// <typeparam name="T">要查找的 ViewModel 类型，必须继承自 <see cref="ObservableObject"/>。</typeparam>
    /// <returns>与指定 ViewModel 类型关联的 View 类型。</returns>
    /// <exception cref="Exception">如果未找到对应的 View 类型，则抛出异常。</exception>
    public static Type FindView<T>() where T : ObservableObject
    {
        return _dict.TryGetValue(typeof(T), out var viewType)
            ? viewType
            : throw new Exception("未找到对应的view类型");
    }
}

/// <summary>
/// 用于标记 View 类型，并指定其关联的 ViewModel 类型。
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NavigateViewModelAttribute : Attribute
{
    /// <summary>
    /// 获取与该特性关联的 ViewModel 类型。
    /// </summary>
    internal Type ViewModelType { get; }

    /// <summary>
    /// 初始化 <see cref="NavigateViewModelAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="viewModelType">与该特性关联的 ViewModel 类型。</param>
    public NavigateViewModelAttribute(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }
}
