using System.Reflection;

#if ZWCAD
// 中望 CAD 需要显式声明扩展入口和命令类，否则加载后不会识别
[assembly: ExtensionApplication(typeof(Oddob.Acad.Init))]
[assembly: CommandClass(typeof(Oddob.Acad.LispFunc))]
#endif

namespace Oddob.Acad;

public class Init : IExtensionApplication
{
    public void Initialize()
    {
        // 必须最先注册：AutoCAD 2025+（.NET 8）下依赖程序集不会自动从插件目录解析
        AssemblyResolver.Register();
        IdleAction.Add(InitializePrivate);
    }

    public void InitializePrivate()
    {
#if !ZWCAD
        var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (!string.IsNullOrWhiteSpace(dir))
        {
            // 先把插件目录加入受信任路径，再加载依赖
            TrustedPathEx.Add(dir);
        }
#endif

        Service.Initialize.Setup(Assembly.GetExecutingAssembly());
        DocEvent.SetUp();
    }

    public void Terminate()
    {
    }
}