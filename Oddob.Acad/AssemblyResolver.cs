using System.Reflection;
using System.Threading;
#if NET
using System.Runtime.Loader;
#endif

namespace Oddob.Acad;

/// <summary>
/// 从插件所在目录解析依赖程序集
/// AutoCAD 2025+ 使用 .NET 8 运行时加载插件，默认的 LoadFrom 探测可能找不到插件目录下的依赖，
/// 需要在此显式接管。
/// </summary>
internal static class AssemblyResolver
{
    private static readonly string BaseDir =
        Path.GetDirectoryName(typeof(AssemblyResolver).Assembly.Location) ?? string.Empty;

    /// <summary>
    /// 正在解析的程序集名称，避免递归解析
    /// </summary>
    private static readonly HashSet<string> Resolving = new(StringComparer.OrdinalIgnoreCase);

    private static int _registered;

#if NET
    /// <summary>
    /// 宿主已加载同名但不同版本的程序集时，默认上下文无法再加载插件自带的版本
    /// （FileLoadException 0x80131621），此时退到独立上下文加载。
    /// </summary>
    private static readonly AssemblyLoadContext PluginContext = new("OddobPlugin", false);
#endif

    /// <summary>
    /// 注册程序集解析事件，可重复调用
    /// </summary>
    public static void Register()
    {
        if (Interlocked.Exchange(ref _registered, 1) == 1)
            return;
        AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
#if NET
        // 插件可能被宿主加载到非默认上下文，两处都要接管
        AssemblyLoadContext.Default.Resolving += OnResolving;
        var context = AssemblyLoadContext.GetLoadContext(typeof(AssemblyResolver).Assembly);
        if (context is not null && !ReferenceEquals(context, AssemblyLoadContext.Default))
            context.Resolving += OnResolving;
#endif
    }

    private static Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args)
    {
        try
        {
            return Resolve(new AssemblyName(args.Name));
        }
        catch (Exception)
        {
            return null;
        }
    }

#if NET
    private static Assembly? OnResolving(AssemblyLoadContext context, AssemblyName assemblyName) =>
        Resolve(assemblyName);
#endif

    private static Assembly? Resolve(AssemblyName name)
    {
        var simpleName = name.Name;
        if (string.IsNullOrEmpty(simpleName))
            return null;
        // 资源程序集找不到时按默认逻辑处理
        if (simpleName.EndsWith(".resources", StringComparison.OrdinalIgnoreCase))
            return null;

        var loaded = FindLoaded(simpleName);

        // 已加载完全相同的版本（含插件自己加载过的）时直接复用
        if (loaded is not null && loaded.GetName().Version == name.Version)
            return loaded;

        if (!string.IsNullOrEmpty(BaseDir))
        {
            var path = Path.Combine(BaseDir, simpleName + ".dll");
            if (File.Exists(path))
            {
                lock (Resolving)
                {
                    if (!Resolving.Add(simpleName))
                        return loaded;
                }

                try
                {
                    var assembly = Load(path);
                    if (assembly is not null)
                        return assembly;
                }
                finally
                {
                    lock (Resolving)
                    {
                        Resolving.Remove(simpleName);
                    }
                }
            }
        }

        // 插件自带的版本加载不了时，退回宿主已加载的同名程序集，避免整个流程失败
        return loaded;
    }

    private static Assembly? FindLoaded(string simpleName)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (string.Equals(assembly.GetName().Name, simpleName, StringComparison.OrdinalIgnoreCase))
                return assembly;
        }

        return null;
    }

    private static Assembly? Load(string path)
    {
#if NET
        try
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
        }
        catch (Exception ex) when (ex is FileLoadException or BadImageFormatException)
        {
            // 默认上下文已有同名不同版本的程序集，改用独立上下文加载插件自带的版本
            try
            {
                return PluginContext.LoadFromAssemblyPath(path);
            }
            catch (Exception)
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
#else
        try
        {
            return Assembly.LoadFrom(path);
        }
        catch (Exception)
        {
            return null;
        }
#endif
    }
}
