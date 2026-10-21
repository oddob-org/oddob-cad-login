namespace Oddob.Acad;

[SingletonDI(typeof(ILispService))]
internal class LispService : ILispService
{
    public void Setq(string symbol, string value)
    {
        if (Acap.DocumentManager.MdiActiveDocument is not { } doc)
            return;
        try
        {
#if ZWCAD
            // 中望 CAD 未导出 acedEvaluateLisp；SendStringToExecute 会把 (setq ...) 的返回值
            // 回显成 nil，这里改用托管接口直接写 LISP 符号，不经过命令行
            doc.SetLispSymbol(symbol, value == "T");
#else
            var func = "(setq " + symbol + " " + value + ")";
            _ = AcedEvaluateLisp(func, out var rb);
#endif
            // doc.Editor.WriteMessage(rb + "\n");
            // if (rb != IntPtr.Zero)
            // {
            //     var tvs = Marshaler.ResbufToTypedValues(rb);
            //     foreach (var typedValue in tvs)
            //     {
            //         doc.Editor.WriteMessage(typedValue.TypeCode + " " + typedValue.Value + "\n");
            //     }
            // }
        }
        catch (Exception)
        {
            // doc.Editor.WriteMessage("设置失败");
        }
    }

#if !ZWCAD
    // 高版本此接口不能使用lisp(command "xx"),但是可以直接在自动运行接口上
    [DllImport("accore.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl,
        EntryPoint = "?acedEvaluateLisp@@YAHPEB_WAEAPEAUresbuf@@@Z")]
    [System.Security.SuppressUnmanagedCodeSecurity] // 初始化默认值
    private static extern int AcedEvaluateLisp(string lispLine, out IntPtr result);
#endif
}